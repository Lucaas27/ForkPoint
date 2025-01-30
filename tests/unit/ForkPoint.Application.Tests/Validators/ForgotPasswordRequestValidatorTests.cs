using FluentAssertions;
using FluentValidation.TestHelper;
using ForkPoint.Application.Models.Handlers.ForgotPassword;
using ForkPoint.Application.Validators;

namespace ForkPoint.Application.Tests.Validators;

// TestMethod_ExpectedResult_Scenario
public class ForgotPasswordRequestValidatorTests
{
    private readonly ForgotPasswordRequestValidator _validator = new();

    [Fact]
    public void ForgotPasswordRequest_PassesValidation_WhenEmailIsValid()
    {
        // Arrange
        var request = new ForgotPasswordRequest("test@test.com");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ForgotPasswordRequest_FailsValidation_WhenEmailIsInvalid()
    {
        // Arrange
        var request = new ForgotPasswordRequest("testtest.com");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void ForgotPasswordRequest_FailsValidation_WhenEmailIsEmpty()
    {
        // Arrange
        var request = new ForgotPasswordRequest(string.Empty);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}
