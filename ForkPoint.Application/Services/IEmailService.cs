using ForkPoint.Application.Models.Emails;

namespace ForkPoint.Application.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string templateKey, EmailTemplateParameters? parameters);
}