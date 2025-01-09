using ForkPoint.Application.Contexts;
using ForkPoint.Application.Models.Handlers.Logout;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class LogoutHandler(
    ILogger<LogoutHandler> logger,
    UserManager<User> userManager,
    IAuthService authService,
    IUserContext userContext
) : BaseHandler<LogoutRequest, LogoutResponse>
{
    public override async Task<LogoutResponse> Handle(LogoutRequest request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();

        if (user is null)
        {
            logger.LogWarning("User not authenticated - {Email}", user?.Email);
            return new LogoutResponse
            {
                IsSuccess = false,
                Message = "User not authenticated"
            };
        }

        logger.LogInformation("Processing logout request for user {Email}", user.Email);


        var dbUser = await userManager.FindByEmailAsync(user.Email) ??
                     throw new InvalidOperationException("User not found in the database");


        // Invalidate the refresh token for the user to prevent further access to the API.
        // This will force the user to log in again to get a new access token when the current short-lived one expires.
        await authService.InvalidateRefreshToken(dbUser);

        return new LogoutResponse
        {
            IsSuccess = true,
            Message = "Logged out successfully"
        };
    }
}