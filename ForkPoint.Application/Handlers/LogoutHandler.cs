using ForkPoint.Application.Models.Handlers.Logout;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class LogoutHandler(
    ILogger<LogoutHandler> logger,
    UserManager<User> userManager,
    IAuthService authService
) : BaseHandler<LogoutRequest, LogoutResponse>
{
    public override async Task<LogoutResponse> Handle(LogoutRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing logout request for user {Email}", request.Email);

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            logger.LogWarning("User {Email} not found", request.Email);
            return new LogoutResponse
            {
                IsSuccess = false,
                Message = "User not found"
            };
        }

        // Invalidate the refresh token for the user to prevent further access to the API.
        // This will force the user to log in again to get a new access token when the current short-lived one expires.
        await authService.InvalidateRefreshToken(user);

        return new LogoutResponse
        {
            IsSuccess = true,
            Message = "Logged out successfully"
        };
    }
}