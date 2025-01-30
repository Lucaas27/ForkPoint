using FluentValidation.TestHelper;
using ForkPoint.Application.Models.Handlers.LoginUser;
using ForkPoint.Application.Validators;

namespace ForkPoint.Application.Tests.Validators;

// TestMethod__ExpectedResult__Scenario
public class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _validator = new();

    [Fact]
    public void LoginRequest_PassesValidation_WhenEmailIsValid()
    {
        // Arrange
        var model = new LoginRequest("test@example.com", "password123");

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void LoginRequest_FailsValidation_WhenEmailIsInvalid()
    {
        // Arrange
        var model = new LoginRequest("invalid-email", "password123");

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Invalid email address");
    }

    [Fact]
    public void LoginRequest_FailsValidation_WhenPasswordIsTooShort()
    {
        // Arrange
        var model = new LoginRequest("test@example.com", "12345");

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Password must be at least 6 characters long");
    }

}
