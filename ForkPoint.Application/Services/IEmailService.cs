using ForkPoint.Application.Models.Emails;

namespace ForkPoint.Application.Services;

public interface IEmailService
{
    Task SendEmailAsync(IEmailTemplate emailTemplate);
}