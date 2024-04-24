using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Configurations;

public static class MigrationConfigurations
{
    public static void MigrateSecurityDbContext(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            var securityContext = services.GetRequiredService<SecurityDbContext>();
            var environment = services.GetRequiredService<IWebHostEnvironment>();

            if (environment.IsDevelopment())
            {
                securityContext.Database.Migrate();
            }
            else
            {
                securityContext.Database.EnsureCreated();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error has occurred in the security database migration process.");
        }
    }
}