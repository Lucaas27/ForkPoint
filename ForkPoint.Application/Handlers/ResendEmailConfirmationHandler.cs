using ForkPoint.Application.Factories;
using ForkPoint.Application.Models.Handlers.ResendEmailConfirmation;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ForkPoint.Domain.Repositories;

namespace ForkPoint.Application.Handlers;

public class
    ResendEmailConfirmationHandler(
        ILogger<ResendEmailConfirmationHandler> logger,
        IUserRepository userRepository,
        IEmailService emailService,
        IEmailTemplateFactory emailTemplateFactory
    )
    : IRequestHandler<ResendEmailConfirmationRequest, ResendEmailConfirmationResponse>
{
    public async Task<ResendEmailConfirmationResponse> Handle(
        ResendEmailConfirmationRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Resending account confirmation email to {Email}...", request.Email);

        var user = await userRepository.FindByEmailAsync(request.Email) ??
                   throw new NotFoundException(nameof(User), request.Email);

        if (user.EmailConfirmed)
        {
            return new ResendEmailConfirmationResponse
            {
                IsSuccess = false,
                Message = "Account has already been verified for this user."
            };
        }

        var emailToken = await userRepository.GenerateEmailConfirmationTokenAsync(user);
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