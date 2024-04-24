using Ardalis.Result;
using Inventiry.Aplication.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Inventiry.Aplication.Services;

public class TokenManagerService : ITokenManagerService
{
    private readonly IConfiguration _configuration;

    public TokenManagerService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Result<string> GenerateAccessToken(IdentityUser user)
    {
        try
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Jwt:SecretKey"]!));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // Claims
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, $"{user.UserName}"),
                new(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new(ClaimTypes.Email, $"{user.Email}")
            };

            // Payload
            var payload = new JwtPayload(
                _configuration["Authentication:Jwt:Issuer"],
                _configuration["Authentication:Jwt:Audience"],
                claims, DateTime.Now, DateTime.UtcNow.AddMinutes(10));

            var securityToken = new JwtSecurityToken(header, payload);
            var securityTokenValue = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return Result<string>.Success(securityTokenValue);
        }
        catch (Exception ex)
        {
            return Result<string>.Error(ex.Message);
        }
    }
}