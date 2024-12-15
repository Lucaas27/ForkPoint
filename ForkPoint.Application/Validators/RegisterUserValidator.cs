using FluentValidation;
using ForkPoint.Application.Models.Handlers.RegisterUser;

namespace ForkPoint.Application.Validators;

// Most validation will be done by Identity
public class RegisterUserValidator : AbstractValidator<RegisterRequest>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}