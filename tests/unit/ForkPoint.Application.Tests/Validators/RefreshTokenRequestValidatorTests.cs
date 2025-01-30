using FluentValidation.TestHelper;
using ForkPoint.Application.Models.Handlers.RefreshToken;
using ForkPoint.Application.Validators;

namespace ForkPoint.Application.Tests.Validators;

public class RefreshTokenRequestValidatorTests
{
    private readonly RefreshTokenRequestValidator _validator = new();

    [Fact]
    public void RefreshTokenRequest_FailsValidation_WhenAccessTokenIsEmpty()
    {
        var model = new RefreshTokenRequest(string.Empty, "validRefreshToken");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AccessToken)
              .WithErrorMessage("Access token is required");
    }

    [Fact]
    public void RefreshTokenRequest_FailsValidation_WhenRefreshTokenIsEmpty()
    {
        var model = new RefreshTokenRequest("validAccessToken", string.Empty);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.RefreshToken)
              .WithErrorMessage("Refresh token is required");
    }

    [Fact]
    public void RefreshTokenRequest_PassesValidation_WhenAccessTokenAndRefreshTokenAreProvided()
    {
        var model = new RefreshTokenRequest("validAccessToken", "validRefreshToken");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.AccessToken);
        result.ShouldNotHaveValidationErrorFor(x => x.RefreshToken);
    }
}
