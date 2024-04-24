using Ardalis.Result;
using Inventiry.Aplication.Interfaces;
using Inventory.Domain.Dtos;
using Inventory.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Inventiry.Aplication.Services;

public class UserService : IUserService
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenManagerService _tokenManagerService;
    private readonly SecurityDbContext _securityContext;

    public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        ITokenManagerService tokenManagerService, SecurityDbContext securityDbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenManagerService = tokenManagerService;
        _securityContext = securityDbContext;
    }

    public async Task<Result<LoginDto>> LoginAsync(string email, string password)
    {
        //Verify that the email exists in the database
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null) return Result.Invalid(new List<ValidationError> { new ValidationError { ErrorMessage = "Email or Password incorrect" } });


        //Verify that the password is correct to log in
        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
        if (!result.Succeeded) return Result.Invalid(new List<ValidationError> { new() { ErrorMessage = "Email or Password incorrect" } });

        var accessToken = _tokenManagerService.GenerateAccessToken(user).Value;

        var loginDto = new LoginDto
        {
            AccessToken = accessToken,
            Tenants = await GetTenants(user.Id)
        };

        return Result.Success(loginDto);
    }

    public async Task<List<string>> GetTenants(string userId)
    {
        List<string> organizationSlugTenants = _securityContext.OrganizationUsers
            .Include(organizationUser => organizationUser.Organization)
            .Where(organizationUser => organizationUser.UserId == userId)
            .Select(organizationUser => organizationUser.Organization.SlugTenant)
            .ToList();

        return organizationSlugTenants;
    }

    public async Task<Result> Create(string email, string password, string slugTenant)
    {
        IdentityUser user = new();

        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;
        user.PhoneNumberConfirmed = true;
        user.TwoFactorEnabled = false;
        user.LockoutEnabled = false;

        var existEmail = await _userManager.FindByEmailAsync(user.Email);

        if (existEmail is not null) return Result.Invalid(new List<ValidationError> { new() { ErrorMessage = "Email not available" } });

        var resultCreateUser = await _userManager.CreateAsync(user, password);

        if (resultCreateUser.Succeeded)
        {
            var organizartion = _securityContext.Organizations.Where(organization => organization.SlugTenant == slugTenant).FirstOrDefault();

            await _securityContext.OrganizationUsers.AddAsync(new()
            {
                OrganizationId = organizartion.Id,
                UserId = user.Id
            });

            await _securityContext.SaveChangesAsync();

            return Result.Success();
        }

        return Result.Invalid(new List<ValidationError> { new() { ErrorMessage = string.Join(",", resultCreateUser.Errors.Select(error => error.Description)) } });
    }
}