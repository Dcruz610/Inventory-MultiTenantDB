using Inventiry.Aplication.Interfaces;
using Inventory.API.Requests;
using Inventory.API.Responses;
using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inventory.API.Controllers;

[ApiController]
[Authorize]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;
    ApiResponse response = new();

    [HttpGet(Routes.Products.GetProducts)]
    [SwaggerOperation(
        Summary = "Products",
        Description = "This endpoint is used to get all products",
        OperationId = "Products.GetAll",
        Tags = new[] { "Products" })
    ]
    public async Task<ActionResult<ApiResponse>> GetAsync()
    {
        var result = await _productService.GetProducts();

        if (result.IsSuccess)
        {
            response = new()
            {
                Status = "200",
                StatusText = "GET Request successful.",
                Data = result.Value
            };

            return Ok(response);
        }
        else
        {
            response = new()
            {
                Status = "400",
                StatusText = "GET Request badRequest.",
                Data = result.ValidationErrors.Select(x => x.ErrorMessage).ToArray()
            };

            return Ok(response);
        }
    }

    [HttpGet(Routes.Products.GetProduct)]
    [SwaggerOperation(
      Summary = "Products",
      Description = "This endpoint is used to get a product by ID",
      OperationId = "Products.GetById",
      Tags = new[] { "Products" })
    ]
    public async Task<ActionResult<ApiResponse>> Get([FromRoute] string id)
    {
        var result = await _productService.GetProductById(id);

        if (result.IsSuccess)
        {
            response = new()
            {
                Status = "200",
                StatusText = "GET Request successful.",
                Data = result.Value
            };

            return Ok(response);
        }
        else
        {
            response = new()
            {
                Status = "400",
                StatusText = "GET Request badRequest.",
                Data = result.ValidationErrors.Select(x => x.ErrorMessage).ToArray()
            };

            return Ok(response);
        }
    }

    [HttpPost(Routes.Products.PostProduct)]
    [SwaggerOperation(
      Summary = "Products",
      Description = "This endpoint is used to create a new product",
      OperationId = "Products.Create",
      Tags = new[] { "Products" })
    ]
    public async Task<ActionResult<ApiResponse>> PostAsync([FromBody] PostProductRequest request)
    {
        var product = new Product() { Name = request.Name, Price = request.Price };

        var result = await _productService.CreateProductAsync(product);

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

    [HttpPut(Routes.Products.PutProduct)]
    [SwaggerOperation(
      Summary = "Products",
      Description = "This endpoint is used to update a product",
      OperationId = "Products.Update",
      Tags = new[] { "Products" })
    ]
    public async Task<ActionResult<ApiResponse>> PutAsync([FromRoute] string id, [FromBody] PutProductRequest request)
    {
        var product = new Product() { Id = Guid.Parse(id), Name = request.Name, Price = request.Price };

        var result = await _productService.UpdateProductAsync(product);

        if (result.IsSuccess)
        {
            response = new()
            {
                Status = "200",
                StatusText = "PUT Request successful.",
                Data = "Product successfully updated"
            };

            return Ok(response);
        }
        else
        {
            response = new()
            {
                Status = "400",
                StatusText = "PUT Request badRequest.",
                Data = result.ValidationErrors.Select(x => x.ErrorMessage).ToArray()
            };

            return Ok(response);
        }
    }

    [HttpDelete(Routes.Products.DeleteProduct)]
    [SwaggerOperation(
      Summary = "Products",
      Description = "This endpoint is used to delete a product",
      OperationId = "Products.Delete",
      Tags = new[] { "Products" })
    ]
    public async Task<ActionResult<ApiResponse>> DeleteAsync(string id)
    {
        var result = await _productService.DeleteProductAsync(id);

        if (result.IsSuccess)
        {
            response = new()
            {
                Status = "200",
                StatusText = "DELETE Request successful.",
                Data = "Product successfully deleted"
            };

            return Ok(response);
        }
        else
        {
            response = new()
            {
                Status = "400",
                StatusText = "DELETE Request badRequest.",
                Data = result.ValidationErrors.Select(x => x.ErrorMessage).ToArray()
            };

            return Ok(response);
        }
    }
}