using ForkPoint.Application.Factories;
using ForkPoint.Application.Models.Handlers.RegisterUser;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ForkPoint.Domain.Repositories;

namespace ForkPoint.Application.Handlers;

public class RegisterHandler(
    ILogger<RegisterHandler> logger,
    IUserRepository userRepository,
    IEmailService emailService,
    IEmailTemplateFactory emailTemplateFactory
) : IRequestHandler<RegisterRequest, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Registering user with email {Email}...", request.Email);

        var user = new User { UserName = request.Email, Email = request.Email };
        var result = await userRepository.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return LogAndReturnError("Failed to register user", request.Email, result.Errors);
        }

        var roleResult = await userRepository.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            return LogAndReturnError("Failed to assign role to user", request.Email, roleResult.Errors);
        }

        var emailToken = await userRepository.GenerateEmailConfirmationTokenAsync(user);
        logger.LogInformation("Email confirmation token generated");

        var emailTemplate = emailTemplateFactory.CreateEmailConfirmationTemplate(request.Email, emailToken);

        await emailService.SendEmailAsync(emailTemplate);

        logger.LogInformation("User with email {Email} registered successfully", request.Email);

        return new RegisterResponse
        {
            IsSuccess = true,
            Message =
                $"User registered successfully. Please confirm your account in the email sent to {request.Email}. Token: {emailToken}"
        };
    }

    private RegisterResponse LogAndReturnError(string message, string email, IEnumerable<IdentityError> errors)
    {
        var errorMessages = string.Join(Environment.NewLine, errors.Select(e => e.Description));
        logger.LogError("{Message} with email {Email}. Error: {Error}", message, email, errorMessages);

        return new RegisterResponse
        {
            IsSuccess = false,
            Message = $"{message}. {errorMessages}"
        };
    }
}