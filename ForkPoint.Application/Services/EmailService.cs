using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Services;

public class EmailService(ILogger<EmailService> logger, IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(string receptor, string subject, string message)
    {
        logger.LogInformation("Sending email to {Receptor}...", receptor);

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                                               | SecurityProtocolType.Tls11
                                               | SecurityProtocolType.Tls;

        var sender = configuration["EmailConfig:From"] ??
                     throw new ArgumentNullException(nameof(configuration), "EmailConfig:From");

        var smtpServer = configuration["EmailConfig:Server"] ??
                         throw new ArgumentNullException(nameof(configuration), "EmailConfig:Server");

        var port = int.TryParse(configuration["EmailConfig:Port"], out var p)
            ? p
            : throw new ArgumentNullException(nameof(configuration), "EmailConfig:Port");

        var password = configuration["EmailConfig:Password"] ??
                       throw new ArgumentNullException(nameof(configuration), "EmailConfig:Password");

        var client = new SmtpClient(smtpServer, port);
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(sender, password);
        client.EnableSsl = true;

        var mail = new MailMessage(sender, receptor, subject, message);

        await client.SendMailAsync(mail);
    }
}