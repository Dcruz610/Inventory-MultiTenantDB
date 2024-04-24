using Ardalis.Result;
using Inventiry.Aplication.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventiry.Aplication.Services;

public class ProductService(ApplicationDbContext dbContext) : IProductService
{
    private readonly ApplicationDbContext _context = dbContext;

    public async Task<Result<List<Product>>> GetProducts()
    {
        try
        {
            var products = await _context.Products.ToListAsync();

            return Result.Success(products);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }

    public async Task<Result<Product>> GetProductById(string id)
    {
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(product => product.Id.ToString() == id);

            return Result.Success(product);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }

    public async Task<Result<Product>> CreateProductAsync(Product product)
    {
        try
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return Result.Success(product);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }

    public async Task<Result<Product>> UpdateProductAsync(Product product)
    {
        try
        {
            var productFound = await GetProductById(product.Id.ToString());

            if (productFound.Value is null) return Result.Invalid(new ValidationError() { ErrorMessage = "Product not found" });

            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return Result.Success(product);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }

    public async Task<Result> DeleteProductAsync(string id)
    {
        try
        {
            var productFound = await GetProductById(id);

            if (productFound.Value is null) return Result.Invalid(new ValidationError() { ErrorMessage = "Product not found" });

            _context.Products.Remove(productFound.Value);
            _context.SaveChanges();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}