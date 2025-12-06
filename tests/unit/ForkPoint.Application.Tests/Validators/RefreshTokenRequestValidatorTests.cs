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
        var model = new RefreshTokenRequest(string.Empty);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AccessToken)
              .WithErrorMessage("Access token is required");
    }
    [Fact]
    public void RefreshTokenRequest_PassesValidation_WhenAccessTokenIsProvided()
    {
        var model = new RefreshTokenRequest("validAccessToken");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.AccessToken);
    }
}
