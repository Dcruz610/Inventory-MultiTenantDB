using Inventory.Domain.Entities;

namespace Inventory.Infrastructure.Providers;

public class TenantProvider : ITenantProvider
{
    private static Organization _tenant;

    public Organization GetTenant()
    {
        return _tenant;
    }

    public void SetTenant(Organization tenant)
    {
        _tenant = tenant;
    }
}