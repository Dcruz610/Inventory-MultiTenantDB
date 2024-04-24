using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Data;

public class SecurityDbContext : IdentityDbContext
{
    public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrganizationUser>().HasKey(organizationUser => new { organizationUser.OrganizationId, organizationUser.UserId });
    }

    public virtual DbSet<Organization> Organizations => Set<Organization>();
    public virtual DbSet<OrganizationUser> OrganizationUsers => Set<OrganizationUser>();
}