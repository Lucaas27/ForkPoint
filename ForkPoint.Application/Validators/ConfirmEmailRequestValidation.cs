using FluentValidation;
using ForkPoint.Application.Models.Handlers.EmailConfirmation;

namespace ForkPoint.Application.Validators;

public class ConfirmEmailRequestValidation : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidation()
    {
        RuleFor(x => x.TokenEmail)
            .NotEmpty()
            .WithMessage("TokenEmail is required");

        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty()
            .WithMessage("Email is required");
    }
}