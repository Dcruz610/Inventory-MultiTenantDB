using Ardalis.Result;
using Microsoft.AspNetCore.Identity;

namespace Inventiry.Aplication.Interfaces;

public interface ITokenManagerService
{
    Result<string> GenerateAccessToken(IdentityUser user);
}