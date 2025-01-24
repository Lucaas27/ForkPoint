using ForkPoint.Application.Models.Handlers.EmailConfirmation;
using ForkPoint.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class ConfirmEmailHandler(
    ILogger<ConfirmEmailHandler> logger,
    UserManager<User> userManager
) : IRequestHandler<ConfirmEmailRequest, ConfirmEmailResponse>
{
    public async Task<ConfirmEmailResponse> Handle(
        ConfirmEmailRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Attempting to confirm email with token {Token} for user {Email}", request.Token,
            request.Email);

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            logger.LogError("User with email {Email} not found", request.Email);
            return new ConfirmEmailResponse
            {
                IsSuccess = false,
                Message = "Payload is invalid"
            };
        }


        var result = await userManager.ConfirmEmailAsync(user, request.Token);

        if (!result.Succeeded)
        {
            var errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
            logger.LogError("Failed to confirm email for user with email {Email}", request.Email);
            logger.LogError("Error: {Error}", errors);
            return new ConfirmEmailResponse
            {
                IsSuccess = false,
                Message = $"Failed to confirm email. {errors}"
            };
        }

        logger.LogInformation("Email confirmed for user {Email}", request.Email);

        return new ConfirmEmailResponse
        {
            IsSuccess = true,
            Message = "Email confirmed"
        };
    }
}