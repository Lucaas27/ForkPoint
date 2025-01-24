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

        var principal = authService.GetPrincipalFromToken(request.AccessToken);

        if (principal is null)
        {
            return new RefreshTokenResponse
            {
                IsSuccess = false,
                Message = "Invalid access token"
            };
        }


        var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value ??
                        throw new ArgumentNullException(
                            nameof(request),
                            "Email claim not found");

        var user = await userManager.FindByEmailAsync(userEmail);

        if (user is null)
        {
            return new RefreshTokenResponse
            {
                IsSuccess = false,
                Message = "Invalid user"
            };
        }

        var tokenExists =
            await userManager.GetAuthenticationTokenAsync(user, "CustomRefreshTokenProvider", "RefreshToken") != null;

        var isTokenValid = await authService.ValidateRefreshToken(user, request.RefreshToken);

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

        // Get new refresh token
        var refreshToken = await authService.GenerateRefreshToken(user);

        return new RefreshTokenResponse(token, refreshToken, expiry) { IsSuccess = true, Message = "Token refreshed" };
    }
}
