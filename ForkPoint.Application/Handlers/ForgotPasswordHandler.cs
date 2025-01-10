using ForkPoint.Application.Models.Emails;
using ForkPoint.Application.Models.Handlers.ForgotPassword;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class ForgotPasswordHandler(
    ILogger<ForgotPasswordHandler> logger,
    UserManager<User> userManager,
    IEmailService emailService,
    IConfiguration configuration
) : BaseHandler<ForgotPasswordRequest, ForgotPasswordResponse>
{
    public override async Task<ForgotPasswordResponse> Handle(
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

        var uri = $"{configuration["ClientURI"]}/account/resetPassword";

        var callback = QueryHelpers.AddQueryString(uri, new Dictionary<string, string>
        {
            { "token", passwordToken },
            { "email", request.Email }
        }!);

        logger.LogInformation("Token: {token}", passwordToken);

        await emailService.SendEmailAsync(request.Email, "EmailPasswordReset", new EmailTemplateParameters
        {
            Callback = callback,
            Token = passwordToken
        });

        logger.LogInformation("Password reset token sent to {Email}", request.Email);

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