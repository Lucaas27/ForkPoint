using FluentValidation;
using ForkPoint.Application.Models.Handlers.LoginUser;

namespace ForkPoint.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(x => x.Password)
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long");
    }
}