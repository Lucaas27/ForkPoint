using ForkPoint.Application.Models.Emails;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace ForkPoint.Application.Factories;

public class EmailTemplateFactory(IConfiguration config) : IEmailTemplateFactory
{
    public EmailConfirmationTemplate CreateEmailConfirmationTemplate(string destination, string token)
    {
        var clientUri = config["ClientURI"] ?? throw new ArgumentNullException(nameof(config), "ClientURI is missing.");
        var callback = QueryHelpers.AddQueryString($"{clientUri}/account/verify", new Dictionary<string, string>
        {
            { "token", token },
            { "email", destination }
        }!);

        return new EmailConfirmationTemplate(callback, destination, token);
    }

    public PasswordResetTemplate CreatePasswordResetTemplate(string destination, string token)
    {
        var clientUri = config["ClientURI"] ?? throw new ArgumentNullException(nameof(config), "ClientURI is missing.");
        var callback = QueryHelpers.AddQueryString($"{clientUri}/account/reset-password", new Dictionary<string, string>
        {
            { "token", token },
            { "email", destination }
        }!);
        return new PasswordResetTemplate(callback, destination);
    }
}