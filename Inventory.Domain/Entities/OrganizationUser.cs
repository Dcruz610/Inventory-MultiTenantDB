using Microsoft.AspNetCore.Identity;

namespace Inventory.Domain.Entities;

public class OrganizationUser
{
    public string UserId { get; set; }
    public Guid OrganizationId { get; set; }

    public Organization Organization { get; set; }
    public IdentityUser User { get; set; }
}