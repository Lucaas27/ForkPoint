using FluentValidation.TestHelper;
using ForkPoint.Application.Models.Handlers.ResetPassword;
using ForkPoint.Application.Validators;

namespace ForkPoint.Application.Tests.Validators;

// TestMethod_ExpectedResult_Scenario
public class ResetPasswordRequestValidatorTests
{
    private readonly ResetPasswordRequestValidator _validator = new();

    [Fact]
    public void ResetPasswordRequest_FailsValidation_WhenPasswordsDoNotMatch()
    {
        // Arrange
        var request = new ResetPasswordRequest(
        "Password123",
        "Password456",
        "test@example.com",
        "1234567890");


        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword)
              .WithErrorMessage("The Password and confirmation password do not match.");
    }

    [Fact]
    public void ResetPasswordRequest_PassesValidation_WhenPasswordsMatch()
    {
        // Arrange
        var request = new ResetPasswordRequest(
        "Password123",
        "Password123",
        "test@example.com",
        "1234567890");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ConfirmPassword);
    }

    [Fact]
    public void ResetPasswordRequest_FailsValidation_WhenPasswordIsEmpty()
    {
        // Arrange
        var request = new ResetPasswordRequest(
        new string(' ', 8),
        new string(' ', 8),
        "test@example.com",
        "1234567890");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void ResetPasswordRequest_FailsValidation_WhenTokenIsEmpty()
    {
        // Arrange
        var request = new ResetPasswordRequest(
        new string('t', 8),
        new string('t', 8),
        "test@example.com",
        new string(' ', 9));

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Token);
    }

    [Fact]
    public void ResetPasswordRequest_FailsValidation_WhenEmailIsInvalid()
    {
        // Arrange
        var request = new ResetPasswordRequest(
        "Password123",
        "Password123",
        "invalid-email",
        "1234567890");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("'Email' is not a valid email address.");
    }


}
