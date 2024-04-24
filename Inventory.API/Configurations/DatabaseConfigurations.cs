using Inventory.Infrastructure.Data;
using Inventory.Infrastructure.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Configurations;

public static class DatabaseConfigurations
{
    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("SecurityDbConnection")!;

        services.AddDbContext<SecurityDbContext>(options => options.UseSqlServer(connectionString, x => { x.MigrationsAssembly("Inventory.Infrastructure"); }));

        services.AddDbContext<ApplicationDbContext>();

        services.AddTransient<ITenantProvider, TenantProvider>();

        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<SecurityDbContext>();

        return services;
    }
}