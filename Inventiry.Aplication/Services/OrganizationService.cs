using Ardalis.Result;
using Inventiry.Aplication.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Data;
using Inventory.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventiry.Aplication.Services;

public class OrganizationService(SecurityDbContext securityDbContext, IServiceProvider serviceProvider, IConfiguration configurations, ITenantProvider tenantProvider) : IOrganizationService
{
    private readonly SecurityDbContext _securityDbContext = securityDbContext;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IConfiguration _configurations = configurations;
    private readonly ITenantProvider _tenantProvider = tenantProvider;

    public async Task<Result> Create(Organization organization)
    {
        try
        {
            //Creation of the organization
            await _securityDbContext.Organizations.AddAsync(organization);
            await _securityDbContext.SaveChangesAsync();

            //Connection string assignment for tenant behavior
            organization.ConecctionTenant = _configurations.GetConnectionString("ApplicationDbConnection")!.Replace("[TenantId]", organization.Id.ToString());

            _securityDbContext.Organizations.Update(organization);

            await _securityDbContext.SaveChangesAsync();

            _tenantProvider.SetTenant(organization);

            //Create DB Tenant for the new organization
            using IServiceScope scopeTenant = _serviceProvider.CreateScope();
            ApplicationDbContext dbContext = scopeTenant.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.ActivateTenant();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }

    public async Task<Result<Organization>> GetByIdAsync(string organizationId)
    {
        try
        {
            var organization = await _securityDbContext.Organizations
                .Where(organization => organization.Id.ToString() == organizationId).FirstOrDefaultAsync();

            return Result.Success(organization);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }

    public async Task<Result<Organization>> GetBySlugtenant(string slugTenant)
    {
        try
        {
            var organization = await _securityDbContext.Organizations
                .Where(organization => organization.SlugTenant == slugTenant).FirstOrDefaultAsync();

            return Result.Success(organization);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }

    public async Task<Result<OrganizationUser>> VerifyOrganizationUser(string userId, string organizationId)
    {
        try
        {
            var organizationUser = await _securityDbContext.OrganizationUsers
                .Where(organizationUser => organizationUser.OrganizationId == Guid.Parse(organizationId) && organizationUser.UserId == userId).FirstOrDefaultAsync();

            return Result.Success(organizationUser);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}