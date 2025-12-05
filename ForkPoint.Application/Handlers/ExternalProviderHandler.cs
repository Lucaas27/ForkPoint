using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ForkPoint.Application.Models.Handlers.ExternalProviderCallback;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class ExternalProviderHandler(
    ILogger<ExternalProviderHandler> logger,
    IAuthService authService,
    UserManager<User> userManager,
    SignInManager<User> signInManager
) : IRequestHandler<ExternalProviderRequest, ExternalProviderResponse>
{
    public async Task<ExternalProviderResponse> Handle(
        ExternalProviderRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Handling external provider authentication request...");

        // Login with external provider
        var user = await ExternalProviderLogin();

        // Get token
        var token = await authService.GenerateAccessToken(user);

        // Get token expiry
        var expiry = new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo;

        // Generate refresh token and set cookie
        await authService.GenerateRefreshToken(user);

        return new ExternalProviderResponse(token, expiry) { IsSuccess = true };
    }

    private async Task<User> ExternalProviderLogin()
    {
        // Get external login information
        var externalLoginInfo = await signInManager.GetExternalLoginInfoAsync() ?? throw new InvalidOperationException("Error loading external login information.");
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
                        FullName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                        UserName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                        Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
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
            throw new InvalidOperationException(
                $"Failed to create user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
        }

        var roleResult = await userManager.AddToRoleAsync(user, "User");

        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException(
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
            throw new InvalidOperationException("Failed to add external login.");
        }
    }
}
