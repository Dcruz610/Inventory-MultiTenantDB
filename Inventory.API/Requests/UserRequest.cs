using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Requests;

public class UserRequest
{
    [FromBody] public UserBodyRequest User { get; set; }
    [FromRoute] required public string SlugTenant { get; set; }
}

public class UserBodyRequest
{
    required public string Email { get; set; }
    required public string Password { get; set; }
}