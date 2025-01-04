using ForkPoint.Application.Models.Handlers.ResetPassword;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class ResetPasswordHandler(
    ILogger<ResetPasswordHandler> logger,
    UserManager<User> userManager
) : BaseHandler<ResetPasswordRequest, ResetPasswordResponse>
{
    public override async Task<ResetPasswordResponse> Handle(
        ResetPasswordRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Changing password for user {email}", request.Email);

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return new ResetPasswordResponse
            {
                IsSuccess = false,
                Message = "Invalid request"
            };
        }

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (result.Succeeded)
        {
            logger.LogInformation("Password changed successfully");
            return new ResetPasswordResponse
            {
                IsSuccess = true,
                Message = "Password changed successfully"
            };
        }

        var errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));

        return new ResetPasswordResponse
        {
            IsSuccess = false,
            Message = errors
        };
    }
}