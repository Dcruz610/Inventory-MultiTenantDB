namespace Inventory.Domain.Dtos;

public class LoginDto
{
    public string AccessToken { get; set; }
    public List<string> Tenants { get; set; }
}