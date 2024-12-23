using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ForkPoint.Application.Constants;
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
                            nameof(principal),
                            "Email claim not found");

        var user = await userManager.FindByEmailAsync(userEmail);

        if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            return new RefreshTokenResponse
            {
                IsSuccess = false,
                Message = "Invalid refresh token"
            };
        }

        var token = await authService.GenerateToken(user);
        var expiry = new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo;
        var refreshToken = authService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(AuthConstants.RefreshTokenExpirationInHours);
        await userManager.UpdateAsync(user);

        return new RefreshTokenResponse(token, refreshToken, expiry) { IsSuccess = true, Message = "Token refreshed" };
    }
}