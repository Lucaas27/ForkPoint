using FluentValidation;
using ForkPoint.Application.Constants;
using ForkPoint.Application.Models.Handlers.RefreshToken;

namespace ForkPoint.Application.Validators;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty()
            .WithMessage("Access token is required")
            .MinimumLength(AuthConstants.AccessTokenMinLength);

        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required")
            .MinimumLength(AuthConstants.RefreshTokenMinLength);
    }
}