using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ForkPoint.Application.Constants;
using ForkPoint.Application.Models.Handlers.ExternalProviderCallback;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class ExternalProviderHandler(
    ILogger<ExternalProviderHandler> logger,
    IAuthService authService,
    UserManager<User> userManager,
    SignInManager<User> signInManager
) : BaseHandler<ExternalProviderRequest, ExternalProviderResponse>
{
    public override async Task<ExternalProviderResponse> Handle(
        ExternalProviderRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Handling external provider authentication request...");

        // Login with external provider
        var user = await ExternalProviderLogin();

        // Get token
        var token = await authService.GenerateToken(user);

        // Get token expiry
        var expiry = new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo;

        // Get refresh token
        var refreshToken = authService.GenerateRefreshToken();

        // Update user with refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(AuthConstants.RefreshTokenExpirationInHours);
        await userManager.UpdateAsync(user);

        return new ExternalProviderResponse(token, refreshToken, expiry) { IsSuccess = true };
    }

    private async Task<User> ExternalProviderLogin()
    {
        // Get external login information
        var externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo == null)
        {
            throw new Exception("Error loading external login information.");
        }

        var claims = externalLoginInfo.Principal.Claims.ToList();
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var provider = externalLoginInfo.LoginProvider;
        var providerKey = externalLoginInfo.ProviderKey;

        // First try to find user by external login
        var externalUser = await userManager.FindByLoginAsync(provider, providerKey);

        // Find internal user
        var internalUser = await userManager.FindByEmailAsync(email!);

        // If there is no external user, create one
        if (externalUser == null)
        {
            switch (internalUser)
            {
                // Create internal user and associate external login if internal user does not exist
                case null:
                    externalUser = new User
                    {
                        FullName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                                   ?? throw new ArgumentNullException(nameof(claims), "Name claim is missing."),
                        UserName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                                   ?? throw new ArgumentNullException(nameof(claims), "Email claim is missing."),
                        Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                                ?? throw new ArgumentNullException(nameof(claims), "Email claim is missing."),
                        EmailConfirmed = true
                    };

                    await CreateInternalUserAsync(externalUser);
                    await CreateExternalUserAssociationAsync(externalUser, provider, providerKey);
                    return externalUser;
                default:
                    // Associate external login with an existent internal user
                    // This can happen if user registered with email and then logged in with external provider
                    await CreateExternalUserAssociationAsync(internalUser, provider, providerKey);
                    internalUser.EmailConfirmed = true;
                    await userManager.UpdateAsync(internalUser);
                    return internalUser;
            }
        }

        // Now we have an external user, log in and return it
        await signInManager.ExternalLoginSignInAsync(provider, providerKey, false);
        return externalUser;
    }

    private async Task CreateInternalUserAsync(User user)
    {
        var createResult = await userManager.CreateAsync(user);
        if (!createResult.Succeeded)
        {
            throw new Exception(
                $"Failed to create user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
        }

        var roleResult = await userManager.AddToRoleAsync(user, "User");

        if (!roleResult.Succeeded)
        {
            throw new Exception(
                $"Failed to assign role to user: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
        }
    }

    private async Task CreateExternalUserAssociationAsync(User user, string provider, string providerKey)
    {
        // Create external login association
        var addLoginResult = await userManager.AddLoginAsync(
            user,
            new UserLoginInfo(provider, providerKey, null));

        if (!addLoginResult.Succeeded)
        {
            throw new Exception("Failed to add external login.");
        }
    }
}