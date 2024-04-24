using Ardalis.Result;
using Inventory.Domain.Entities;

namespace Inventiry.Aplication.Interfaces;

public interface IProductService
{
    Task<Result<List<Product>>> GetProducts();
    Task<Result<Product>> GetProductById(string id);
    Task<Result<Product>> CreateProductAsync(Product product);
    Task<Result<Product>> UpdateProductAsync(Product product);
    Task<Result> DeleteProductAsync(string id);
}