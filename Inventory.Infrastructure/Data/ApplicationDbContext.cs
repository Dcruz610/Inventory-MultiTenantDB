using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    private readonly Organization _organization;

    public ApplicationDbContext(ITenantProvider tenantProvider, DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        _organization = tenantProvider.GetTenant();
    }
    public DbSet<Product> Products => Set<Product>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_organization.ConecctionTenant, connectionOptions =>
        {
            connectionOptions.MigrationsAssembly("Inventory.Infrastructure");
        });

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Added && e.Entity is ITenantEntity))
        {
            if (string.IsNullOrEmpty(_organization.Id.ToString()))
            {
                throw new Exception("TenantId not found when creating the record.");
            }

            var entidad = item.Entity as ITenantEntity;
            entidad!.TenantId = _organization.Id.ToString();
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public void ActivateTenant()
    {
        var pendingMigrations = this.Database.GetPendingMigrations();

        if (pendingMigrations.Any())
        {
            this.Database.MigrateAsync().Wait();
        }
    }
}