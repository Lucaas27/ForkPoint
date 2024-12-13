using AutoMapper;
using ForkPoint.Application.Models.Handlers.ExternalProviderCallback;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ForkPoint.Application.Handlers;

public class ExternalProviderHandler(
    ILogger<ExternalProviderHandler> logger,
    IMapper mapper,
    ITokenService tokenService,
    UserManager<User> userManager,
    SignInManager<User> signInManager
    ) : BaseHandler<ExternalProviderRequest, ExternalProviderResponse>(logger, mapper)
{
    public override async Task<ExternalProviderResponse> Handle(ExternalProviderRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling external provider authentication request...");
        string token;

        // Get Google authentication result
        var claims = await AuthenticateAndGetClaims(GoogleDefaults.AuthenticationScheme, request.HttpCxt);

        // First try to find user by external login
        var externalLoginInfo = new UserLoginInfo(GoogleDefaults.AuthenticationScheme, claims["providerKey"], GoogleDefaults.DisplayName);
        var userByExternalLogin = await userManager.FindByLoginAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey);

        // If user already exists, sign in and return token
        if (userByExternalLogin != null)
        {
            await signInManager.SignInAsync(userByExternalLogin, isPersistent: false);
            token = await tokenService.GenerateToken(userByExternalLogin);
            return new ExternalProviderResponse { IsSuccess = true, AccessToken = token };
        }

        // Create new user if not found
        var user = new User
        {
            FullName = claims["name"],
            UserName = claims["email"],
            Email = claims["email"],
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(user);
        if (!createResult.Succeeded) return new ExternalProviderResponse
        {
            IsSuccess = false,
            Message = $"Failed to create user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}"
        };


        // Create external login association
        var addLoginResult = await userManager.AddLoginAsync(user, externalLoginInfo);
        if (!addLoginResult.Succeeded) return new ExternalProviderResponse
        {
            IsSuccess = false,
            Message = "Failed to add external login."
        };

        // Sign in and return token
        await signInManager.SignInAsync(user, isPersistent: false);
        token = await tokenService.GenerateToken(user);

        return new ExternalProviderResponse { IsSuccess = true, AccessToken = token };

    }

    private async Task<Dictionary<string, string>> AuthenticateAndGetClaims(
        string claimType,
        HttpContext requestHttpCxt)
    {
        var authenticateResult = await requestHttpCxt.AuthenticateAsync(claimType);
        if (!authenticateResult.Succeeded) throw new Exception("Error authenticating with external provider.");

        var claims = authenticateResult.Principal.Claims.ToList();
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var providerKey = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(email)) throw new Exception("Email claim not found.");
        if (string.IsNullOrEmpty(providerKey)) throw new Exception("Provider key claim not found.");
        if (string.IsNullOrEmpty(name)) throw new Exception("Name key claim not found.");

        return new Dictionary<string, string>
        {
            { "email", email },
            { "name", name },
            { "providerKey", providerKey }
        };
    }
}
