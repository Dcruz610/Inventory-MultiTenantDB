using Ardalis.Result;
using Inventory.Domain.Entities;

namespace Inventiry.Aplication.Interfaces;

public interface IOrganizationService
{
    Task<Result> Create(Organization organization);
    Task<Result<Organization>> GetByIdAsync(string organizationId);
    Task<Result<Organization>> GetBySlugtenant(string tenantSlugTenant);
    Task<Result<OrganizationUser>> VerifyOrganizationUser(string userId, string organizationId);
}