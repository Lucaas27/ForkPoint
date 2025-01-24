using ForkPoint.Application.Factories;
using ForkPoint.Application.Models.Handlers.ForgotPassword;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class ForgotPasswordHandler(
    ILogger<ForgotPasswordHandler> logger,
    UserManager<User> userManager,
    IEmailService emailService,
    IEmailTemplateFactory emailTemplateFactory
) : IRequestHandler<ForgotPasswordRequest, ForgotPasswordResponse>
{
    public async Task<ForgotPasswordResponse> Handle(
        ForgotPasswordRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Trying to reset user password...");
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return CreateResponse("Please check your email and follow the instructions on how to reset your password.");
        }

        var passwordToken = await userManager.GeneratePasswordResetTokenAsync(user);

        logger.LogInformation("Token: {Token}", passwordToken);

        var emailTemplate = emailTemplateFactory.CreatePasswordResetTemplate(request.Email, passwordToken);

        await emailService.SendEmailAsync(emailTemplate);

        logger.LogInformation("Password reset sent to {Email}", request.Email);

        return CreateResponse("Please check your email and follow the instructions on how to reset your password.");
    }

    private static ForgotPasswordResponse CreateResponse(string message)
    {
        return new ForgotPasswordResponse
        {
            IsSuccess = true,
            Message = message
        };
    }
}