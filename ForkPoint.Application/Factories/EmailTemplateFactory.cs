
using ForkPoint.Application.Models.Emails;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace ForkPoint.Application.Factories;

public class EmailTemplateFactory(IConfiguration config) : IEmailTemplateFactory
{
    public EmailConfirmationTemplate CreateEmailConfirmationTemplate(string destination, string token)
    {
        var clientUri = config["ClientURI"] ?? throw new ArgumentNullException(nameof(config), "ClientURI is missing.");
        var callback = QueryHelpers.AddQueryString($"{clientUri}/auth/confirmEmail", new Dictionary<string, string>
        {
            { "token", token },
            { "email", destination }
        }!);

        return new EmailConfirmationTemplate(callback, destination, token);
    }

    public PasswordResetTemplate CreatePasswordResetTemplate(string destination, string token)
    {
        var clientUri = config["ClientURI"] ?? throw new ArgumentNullException(nameof(config), "ClientURI is missing.");
        var callback = QueryHelpers.AddQueryString($"{clientUri}/auth/resetPassword", new Dictionary<string, string>
        {
            { "token", token },
            { "email", destination }
        }!);
        return new PasswordResetTemplate(callback, destination);
    }
}