using Inventory.Domain.Entities;

namespace Inventory.Infrastructure.Providers;

public interface ITenantProvider
{

    void SetTenant(Organization tenant);

    Organization GetTenant();
}