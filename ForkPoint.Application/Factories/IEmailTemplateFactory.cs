using ForkPoint.Application.Models.Emails;

namespace ForkPoint.Application.Factories;

public interface IEmailTemplateFactory
{
    EmailConfirmationTemplate CreateEmailConfirmationTemplate(string destination, string token);
    PasswordResetTemplate CreatePasswordResetTemplate(string destination, string token);
}