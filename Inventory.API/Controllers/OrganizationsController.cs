using Inventiry.Aplication.Interfaces;
using Inventory.API.Requests;
using Inventory.API.Responses;
using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inventory.API.Controllers;

[ApiController]
public class OrganizationsController(IOrganizationService organizationService) : ControllerBase
{
    private readonly IOrganizationService _organizationService = organizationService;
    ApiResponse response = new();

    [HttpPost(Routes.Organizations.PostOrganization)]
    [SwaggerOperation(
    Summary = "Organizations",
    Description = "This endpoint is used to create a new organization",
    OperationId = "Organization.Create",
    Tags = new[] { "Organization" }),
    ]
    public async Task<ActionResult<ApiResponse>> PostOrganizationAsync([FromBody] OrganizationRequest request)
    {
        Organization organization = new() { Name = request.Name, SlugTenant = request.SlugTenant };

        var result = await _organizationService.Create(organization);

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