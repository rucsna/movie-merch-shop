using Microsoft.AspNetCore.Identity;
using MovieMerchShop.Model;

namespace MovieMerchShop.Service.Authentication;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> RegisterAsync(string email, string userName, string password, DateTime birthDate, string address, string role)
    {
        var user = new ApplicationUser
            { UserName = userName, Email = email, BirthDate = birthDate, Address = address, IsActive = true };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return FailedRegistration(result, email, userName);
        }

        await _userManager.AddToRoleAsync(user, role);
        return new AuthResult(true, email, userName, "");
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var managedUser = await _userManager.FindByEmailAsync(email);
        if (managedUser == null)
        {
            return InvalidEmail(email);
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, password);
        if (!isPasswordValid)
        {
            return InvalidPassword(email, managedUser.UserName);
        }

        if (!managedUser.IsActive)
        {
            return InactiveUser(managedUser.UserName);
        }

        var roles = await _userManager.GetRolesAsync(managedUser);
        var accessToken = _tokenService.CreateToken(managedUser, roles[0]);
        return new AuthResult(true, managedUser.Email, managedUser.UserName, accessToken);
    }

    public async Task<AuthResult> DeactivateAsync(string email, string password)
    {
        var managedUser = await _userManager.FindByEmailAsync(email);
        if (managedUser == null)
        {
            return InvalidEmail(email);
        }
        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, password);
        if (!isPasswordValid)
        {
            return InvalidPassword(email, managedUser.UserName);
        }

        if (!managedUser.IsActive)
        {
            return InactiveUser(managedUser.UserName);
        }
        
        managedUser.IsActive = false;
        await _userManager.UpdateAsync(managedUser);
        return new AuthResult(true, $"", managedUser.UserName, "");
    }

    private static AuthResult FailedRegistration(IdentityResult result, string email, string username)
    {
        var authResult = new AuthResult(false, email, username, "");

        foreach (var error in result.Errors)
        {
            authResult.ErrorMessages.Add(error.Code, error.Description);
        }

        return authResult;
    }
    
    private static AuthResult InvalidEmail(string email)
    {
        var result = new AuthResult(false, email, "", "");
        result.ErrorMessages.Add("Bad credentials", "Invalid email");
        return result;
    }
    
    private static AuthResult InvalidPassword(string email, string userName)
    {
        var result = new AuthResult(false, email, userName, "");
        result.ErrorMessages.Add("Bad credentials", "Invalid password");
        return result;
    }

    private static AuthResult InactiveUser(string userName)
    {
        var result = new AuthResult(false, "", userName, "");
        result.ErrorMessages.Add("Deleted user", "This user is not active anymore.");
        return result;
    }
}