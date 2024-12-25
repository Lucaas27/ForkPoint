namespace ForkPoint.Application.Services;

public interface IEmailService
{
    Task SendEmailAsync(string receptor, string subject, string message);
}