using ForkPoint.Application.Models.Emails;
using ForkPoint.Application.Models.Handlers.RegisterUser;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class RegisterHandler(
    ILogger<RegisterHandler> logger,
    UserManager<User> userManager,
    IEmailService emailService,
    IConfiguration configuration
) : BaseHandler<RegisterRequest, RegisterResponse>
{
    public override async Task<RegisterResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Registering user with email {Email}...", request.Email);

        var user = new User { UserName = request.Email, Email = request.Email };
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return LogAndReturnError("Failed to register user", request.Email, result.Errors);
        }

        var roleResult = await userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            return LogAndReturnError("Failed to assign role to user", request.Email, roleResult.Errors);
        }

        var emailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
        logger.LogInformation("Email confirmation token generated");

        var uri =
            $"{configuration["ClientURI"]}/auth/confirmEmail";

        var callback = QueryHelpers.AddQueryString(uri, new Dictionary<string, string>
        {
            { "token", emailToken },
            { "email", request.Email }
        }!);

        await emailService.SendEmailAsync(request.Email, "EmailConfirmationRequest", new EmailTemplateParameters
        {
            Callback = callback,
            Token = emailToken,
            UserEmail = request.Email
        });

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