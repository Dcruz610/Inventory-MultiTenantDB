using Inventiry.Aplication.Interfaces;
using Inventory.API.Requests;
using Inventory.API.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inventory.API.Controllers;

[ApiController]
public class UsersController : Controller
{
    public readonly IUserService _userService;
    ApiResponse response = new();

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost(Routes.Users.PostLogin)]
    [SwaggerOperation(
        Summary = "Login",
        Description = "This endpoint is used to login",
        OperationId = "User.Login",
        Tags = new[] { "User" }),
    ]
    public async Task<ActionResult<ApiResponse>> LogingAsync(LoginRequest request)
    {
        var result = await _userService.LoginAsync(request.Email, request.Password);

        if (result.IsSuccess)
        {
            response = new()
            {
                Status = "200",
                StatusText = "POST Request successful.",
                Data = result.Value
            };

            return Ok(response);
        }
        else
        {
            response = new()
            {
                Status = "400",
                StatusText = "POST Request badRequest.",
                Data = result.ValidationErrors.Select(x => x.ErrorMessage).ToArray()
            };

            return Ok(response);
        }
    }

    [HttpPost(Routes.Users.PostUser)]
    [SwaggerOperation(
       Summary = "User",
       Description = "This endpoint is used to create a new user",
       OperationId = "User.Create",
       Tags = new[] { "User" }),
   ]
    public async Task<ActionResult<ApiResponse>> PostUser(UserRequest request)
    {
        var result = await _userService.Create(request.User.Email, request.User.Password, request.SlugTenant);

        if (result.IsSuccess)
        {
            response = new()
            {
                Status = "200",
                StatusText = "POST Request successful.",
                Data = result.Value
            };

            return Ok(response);
        }
        else
        {
            response = new()
            {
                Status = "400",
                StatusText = "POST Request badRequest.",
                Data = result.ValidationErrors.Select(x => x.ErrorMessage).ToArray()
            };

            return Ok(response);
        }
    }

}