using ForkPoint.Application.Factories;
using ForkPoint.Application.Models.Handlers.ResendEmailConfirmation;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class
    ResendEmailConfirmationHandler(
        ILogger<ResendEmailConfirmationHandler> logger,
        UserManager<User> userManager,
        IEmailService emailService,
        IEmailTemplateFactory emailTemplateFactory
    )
    : BaseHandler<ResendEmailConfirmationRequest, ResendEmailConfirmationResponse>
{
    public override async Task<ResendEmailConfirmationResponse> Handle(
        ResendEmailConfirmationRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Resending account confirmation email to {Email}...", request.Email);

        var user = await userManager.FindByEmailAsync(request.Email) ??
                   throw new NotFoundException(nameof(User), request.Email);

        var emailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
        logger.LogInformation("Account confirmation token generated");

        var emailTemplate = emailTemplateFactory.CreateEmailConfirmationTemplate(request.Email, emailToken);

        await emailService.SendEmailAsync(emailTemplate);

        logger.LogInformation("Confirmation email re-sent successfully to {Email}", request.Email);

        return new ResendEmailConfirmationResponse
        {
            IsSuccess = true,
            Message =
                $"Please confirm your account in the email sent to {request.Email}. Token: {emailToken}"
        };
    }
}