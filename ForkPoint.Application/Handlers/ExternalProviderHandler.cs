using System.Security.Claims;
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
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling external provider authentication request...");

        // Login with external provider
        var user = await ExternalProviderLogin();

        // Get token
        var token = authService.GenerateToken(user);

        return new ExternalProviderResponse(token) { IsSuccess = true };
    }

    private async Task<User> ExternalProviderLogin()
    {
        // Get external login information
        var externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo == null) throw new Exception("Error loading external login information.");

        var claims = externalLoginInfo.Principal.Claims.ToList();
        var provider = externalLoginInfo.LoginProvider;
        var providerKey = externalLoginInfo.ProviderKey;

        // First try to find user by external login
        var user = await userManager.FindByLoginAsync(provider, providerKey);

        // If user does not exist
        if (user == null)
        {
            // Create new user
            user = new User
            {
                FullName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                           ?? throw new ArgumentNullException(nameof(claims), "Name claim is missing."),
                UserName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                           ?? throw new ArgumentNullException(nameof(claims), "Email claim is missing."),
                Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                        ?? throw new ArgumentNullException(nameof(claims), "Email claim is missing."),
                EmailConfirmed = true
            };

            // Create user and external login association in the database
            await CreateExternalUserAsync(user, provider, providerKey);
        }

        // Sign in
        await signInManager.ExternalLoginSignInAsync(provider, providerKey, false);

        return user;
    }

    private async Task CreateExternalUserAsync(User user, string provider, string providerKey)
    {
        var createResult = await userManager.CreateAsync(user);
        if (!createResult.Succeeded)
            throw new Exception(
                $"Failed to create user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");

        var roleResult = await userManager.AddToRoleAsync(user, "User");

        if (!roleResult.Succeeded)
            throw new Exception(
                $"Failed to assign role to user: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");

        // Create external login association
        var addLoginResult = await userManager.AddLoginAsync(
            user,
            new UserLoginInfo(provider, providerKey, null));

        if (!addLoginResult.Succeeded)
            throw new Exception("Failed to add external login.");
    }
}