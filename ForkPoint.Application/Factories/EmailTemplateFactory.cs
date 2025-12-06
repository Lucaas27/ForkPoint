using ForkPoint.Application.Models.Emails;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace ForkPoint.Application.Factories;

public class EmailTemplateFactory(IConfiguration config) : IEmailTemplateFactory
{
    public EmailConfirmationTemplate CreateEmailConfirmationTemplate(string destination, string token)
    {
        var clientUri = config["ClientURI"] ?? throw new ArgumentNullException(nameof(config), "ClientURI is missing.");
        // Normalize to origin (scheme+host+port) in case ClientURI contains a path
        var clientBase = new Uri(clientUri);
        var origin = $"{clientBase.Scheme}://{clientBase.Authority}";
        // Point to the client confirm route: /confirm
        var callback = QueryHelpers.AddQueryString($"{origin}/confirm", new Dictionary<string, string>
        {
            { "token", token },
            { "email", destination }
        }!);

        return new EmailConfirmationTemplate(callback, destination, token);
    }

    public PasswordResetTemplate CreatePasswordResetTemplate(string destination, string token)
    {
        var clientUri = config["ClientURI"] ?? throw new ArgumentNullException(nameof(config), "ClientURI is missing.");
        // Normalize to origin (scheme+host+port) in case ClientURI contains a path
        var clientBase = new Uri(clientUri);
        var origin = $"{clientBase.Scheme}://{clientBase.Authority}";
        // Point to the client reset password route: /reset-password?token=...&email=...
        var callback = QueryHelpers.AddQueryString($"{origin}/reset-password", new Dictionary<string, string>
        {
            { "token", token },
            { "email", destination }
        }!);
        return new PasswordResetTemplate(callback, destination);
    }
}