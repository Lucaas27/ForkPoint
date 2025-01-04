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
    }
}