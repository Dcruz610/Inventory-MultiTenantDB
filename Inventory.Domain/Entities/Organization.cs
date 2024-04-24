namespace Inventory.Domain.Entities;

public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string SlugTenant { get; set; } = "";
    public string ConecctionTenant { get; set; } = "";

    public List<OrganizationUser>? OrganizationUsers { get; set; }
}