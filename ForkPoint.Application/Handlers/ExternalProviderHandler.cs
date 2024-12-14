using System.Security.Claims;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.ExternalProviderCallback;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class ExternalProviderHandler(
    ILogger<ExternalProviderHandler> logger,
    ITokenService tokenService,
    UserManager<User> userManager,
    SignInManager<User> signInManager
) : BaseHandler<ExternalProviderRequest, ExternalProviderResponse>
{
    public override async Task<ExternalProviderResponse> Handle(
        ExternalProviderRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling external provider authentication request...");
        string token;
        var authenticationScheme = request.AuthenticationScheme ??
                                   throw new ArgumentNullException(nameof(request.AuthenticationScheme),
                                       "AuthenticationScheme is missing.");

        // Get authentication result
        var externalProviderClaims = await AuthenticateAndGetClaims(request.HttpCxt, authenticationScheme);

        // First try to find user by external login
        var externalLoginInfo = new UserLoginInfo(authenticationScheme, externalProviderClaims.ProviderKey, null);
        var userByExternalLogin =
            await userManager.FindByLoginAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey);

        // If user already exists, sign in and return token
        if (userByExternalLogin != null)
        {
            await signInManager.SignInAsync(userByExternalLogin, false);
            token = await tokenService.GenerateToken(userByExternalLogin);
            return new ExternalProviderResponse(token) { IsSuccess = true };
        }

        // Create new user if not found
        var user = new User
        {
            FullName = externalProviderClaims.Name,
            UserName = externalProviderClaims.Email,
            Email = externalProviderClaims.Email,
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(user);
        if (!createResult.Succeeded)
            throw new Exception(
                $"Failed to create user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");

        // Create external login association
        var addLoginResult = await userManager.AddLoginAsync(user, externalLoginInfo);
        if (!addLoginResult.Succeeded)
            throw new Exception("Failed to add external login.");

        // Sign in and return token
        await signInManager.SignInAsync(user, false);
        token = await tokenService.GenerateToken(user);

        return new ExternalProviderResponse(token) { IsSuccess = true };
    }

    private async Task<ExternalProviderClaims> AuthenticateAndGetClaims(
        HttpContext requestHttpCxt, string authenticationScheme)
    {
        var authenticateResult = await requestHttpCxt.AuthenticateAsync(authenticationScheme);

        if (!authenticateResult.Succeeded) throw new Exception("Error authenticating with external provider.");

        var claims = authenticateResult.Principal.Claims.ToList();

        if (claims.Any(c => string.IsNullOrEmpty(c.Value)))
            throw new ArgumentNullException(nameof(claims), "Claim type is missing.");

        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
        var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        var providerKey = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

        return new ExternalProviderClaims(email, name, providerKey);
    }
}