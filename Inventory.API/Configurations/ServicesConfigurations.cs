using Inventiry.Aplication.Interfaces;
using Inventiry.Aplication.Services;

namespace Inventory.API.Configurations;

public static class ServicesConfigurations
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<ITokenManagerService, TokenManagerService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IOrganizationService, OrganizationService>();
        services.AddTransient<IProductService, ProductService>();

        return services;
    }
}