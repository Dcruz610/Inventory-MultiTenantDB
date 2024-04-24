using Inventory.Domain.Interfaces;

namespace Inventory.Domain.Entities;

public class Product : ITenantEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string TenantId { get; set; }
}