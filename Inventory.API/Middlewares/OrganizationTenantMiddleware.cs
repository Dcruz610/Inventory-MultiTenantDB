using Inventiry.Aplication.Interfaces;
using Inventory.Infrastructure.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Inventory.API.Middlewares;

public class OrganizationTenantMiddleware
{
    private readonly RequestDelegate _next;
    IServiceProvider _serviceProvider;

    public OrganizationTenantMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Path.ToString().StartsWith(Routes.Users.PostLogin) &&
            !context.Request.Path.ToString().StartsWith(Routes.Organizations.PostOrganization))
        {
            var route = context.GetRouteData();

            var slugTenant = route.Values["SlugTenant"]?.ToString();

            if (slugTenant == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("SlugTenant is missing");
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var organizationService = scope.ServiceProvider.GetRequiredService<IOrganizationService>();
                    var organization = await organizationService.GetBySlugtenant(slugTenant);

                    if (organization.Value is null)
                    {
                        ProblemDetails problem = new()
                        {
                            Title = "NotFound",
                            Detail = "Organization Tenant not found.",
                            Status = 404,
                            Instance = context.Request.Path.ToString()
                        };

                        await context.Response.WriteAsJsonAsync(problem);

                        return;
                    }

                    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    var organizationUser = await organizationService.VerifyOrganizationUser(userId, organization.Value.Id.ToString());

                    if (organizationUser.Value is null && !context.Request.Path.ToString().Contains("/Users"))
                    {
                        ProblemDetails problem = new()
                        {
                            Title = "Unauthorized",
                            Detail = "The user making the request does not have access to the submitted organization tenant.",
                            Status = 401,
                            Instance = context.Request.Path.ToString()
                        };

                        await context.Response.WriteAsJsonAsync(problem);

                        return;
                    }

                    var tenantProvider = scope.ServiceProvider.GetRequiredService<ITenantProvider>();
                    tenantProvider.SetTenant(organization.Value);
                }
                catch (Exception e)
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(e.Message);
                    return;
                }
            }
        }

        await _next.Invoke(context);
    }
}