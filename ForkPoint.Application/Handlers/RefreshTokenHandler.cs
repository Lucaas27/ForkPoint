using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ForkPoint.Application.Models.Handlers.RefreshToken;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class RefreshTokenHandler(
    ILogger<LoginHandler> logger,
    IAuthService authService,
    UserManager<User> userManager
) : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
{
    public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling refresh token request...");
        User? user = null;
        if (!string.IsNullOrWhiteSpace(request.AccessToken))
        {
            var principal = authService.GetPrincipalFromToken(request.AccessToken);

            if (principal is null)
            {
                return new RefreshTokenResponse
                {
                    IsSuccess = false,
                    Message = "Invalid or expired access token"
                };
            }

            var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value ??
                            throw new ArgumentNullException(
                                nameof(request),
                                "Email claim not found");

        user = await userManager.FindByEmailAsync(userEmail);
        }

        if (string.IsNullOrWhiteSpace(request.AccessToken))
        {
            return new RefreshTokenResponse
            {
                IsSuccess = false,
                Message = "Access token is required in the Authorization header"
            };
        }

        if (user is null)
        {
            return new RefreshTokenResponse
            {
                IsSuccess = false,
                Message = "Invalid user"
            };
        }

        // Ensure there's a refresh token in the request cookie
        var cookieRefreshToken = authService.GetRefreshTokenFromRequest();
        if (string.IsNullOrEmpty(cookieRefreshToken))
        {
            return new RefreshTokenResponse
            {
                IsSuccess = false,
                Message = "Refresh token not provided"
            };
        }

        // Ensure refresh token exists for the user and is valid
        var tokenExists = await userManager.GetAuthenticationTokenAsync(user, "CustomRefreshTokenProvider", "RefreshToken") != null;
        var isTokenValid = await authService.ValidateRefreshToken(user, cookieRefreshToken);

        if (!isTokenValid || !tokenExists)
        {
            return new RefreshTokenResponse
            {
                IsSuccess = false,
                Message = "Invalid refresh token"
            };
        }

        var token = await authService.GenerateAccessToken(user);
        var expiry = new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo;

        // Set new refresh token
        await authService.GenerateRefreshToken(user);

        return new RefreshTokenResponse(token, expiry) { IsSuccess = true, Message = "Token refreshed" };
    }
}
