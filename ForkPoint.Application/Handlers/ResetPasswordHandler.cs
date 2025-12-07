using ForkPoint.Application.Models.Handlers.ResetPassword;
using ForkPoint.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ForkPoint.Domain.Repositories;

namespace ForkPoint.Application.Handlers;

public class ResetPasswordHandler(
    ILogger<ResetPasswordHandler> logger,
    IUserRepository userRepository
) : IRequestHandler<ResetPasswordRequest, ResetPasswordResponse>
{
    public async Task<ResetPasswordResponse> Handle(
        ResetPasswordRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Changing password for user {Email}", request.Email);

        var user = await userRepository.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return new ResetPasswordResponse
            {
                IsSuccess = false,
                Message = "Invalid request"
            };
        }

        var result = await userRepository.ResetPasswordAsync(user, request.Token, request.Password);
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