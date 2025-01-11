using ForkPoint.Application.Models.Emails;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace ForkPoint.Application.Services;

internal class EmailService(
    ILogger<EmailService> logger,
    IConfiguration configuration
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

    public async Task SendEmailAsync(IEmailTemplate emailTemplate)
    {
        logger.LogInformation("Sending {EmailType} to {Receptor}...", emailTemplate.GetType(),
            emailTemplate.Destination);


        var email = CreateEmail(emailTemplate.Destination, emailTemplate.Subject, emailTemplate.Html);

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