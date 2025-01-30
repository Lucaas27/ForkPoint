using FluentValidation;
using ForkPoint.Application.Models.Handlers.ResetPassword;

namespace ForkPoint.Application.Validators;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.ConfirmPassword)
            .Equal(y => y.Password)
            .WithMessage("The Password and confirmation password do not match.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token is required");
    }
}