using ForkPoint.Application.Models.Emails;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace ForkPoint.Application.Services;

public class EmailService(
    ILogger<EmailService> logger,
    IConfiguration configuration,
    Func<string, EmailTemplateParameters?, IEmailTemplate> emailTemplateFactory
) : IEmailService
{
    private readonly string _from = configuration["EmailConfig:From"] ??
                                    throw new ArgumentNullException(nameof(configuration), "EmailConfig:From");

    private readonly string _password = configuration["EmailConfig:Password"] ??
                                        throw new ArgumentNullException(nameof(configuration), "EmailConfig:Password");

    private readonly int _port = int.TryParse(configuration["EmailConfig:Port"], out var p)
        ? p
        : throw new ArgumentNullException(nameof(configuration), "EmailConfig:Port");

    private readonly string _smtpServer = configuration["EmailConfig:Server"] ??
                                          throw new ArgumentNullException(nameof(configuration), "EmailConfig:Server");


    public async Task SendEmailAsync(string to, string templateKey, EmailTemplateParameters? parameters = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(to, nameof(to));
        ArgumentException.ThrowIfNullOrEmpty(templateKey, nameof(templateKey));

        logger.LogInformation("Sending {EmailType} to {Receptor}...", templateKey, to);

        var template = emailTemplateFactory(templateKey, parameters);

        var email = CreateEmail(to, template.Subject, template.Content);

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpServer, _port, true);
        await client.AuthenticateAsync(_from, _password);
        await client.SendAsync(email);
    }

    private MimeMessage CreateEmail(string to, string subject, string content)
    {
        var fromAddress = new MailboxAddress("ForkPoint", _from);
        var toAddress = MailboxAddress.Parse(to);
        var email = new MimeMessage
        {
            From = { fromAddress },
            ReplyTo = { fromAddress },
            To = { toAddress },
            Subject = subject,
            Body = new TextPart(TextFormat.Html) { Text = content }
        };

        return email;
    }
}