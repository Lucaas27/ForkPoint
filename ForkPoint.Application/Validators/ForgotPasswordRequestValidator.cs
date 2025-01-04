using FluentValidation;
using ForkPoint.Application.Models.Handlers.ForgotPassword;

namespace ForkPoint.Application.Validators;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty()
            .WithMessage("Email is required");
    }
}