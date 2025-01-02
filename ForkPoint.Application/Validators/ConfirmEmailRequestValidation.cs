using FluentValidation;
using ForkPoint.Application.Models.Handlers.EmailConfirmation;

namespace ForkPoint.Application.Validators;

public class ConfirmEmailRequestValidation : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidation()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("TokenEmail is required");

        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty()
            .WithMessage("Email is required");
    }
}