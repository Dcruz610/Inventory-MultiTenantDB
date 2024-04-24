using Ardalis.Result;
using Inventory.Domain.Dtos;

namespace Inventiry.Aplication.Interfaces;

public interface IUserService
{
    Task<Result<LoginDto>> LoginAsync(string email, string password);
    Task<List<string>> GetTenants(string userId);
    Task<Result> Create(string email, string password, string slugTenant);
}