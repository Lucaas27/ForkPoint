using FluentValidation.TestHelper;
using ForkPoint.Application.Models.Handlers.RegisterUser;
using ForkPoint.Application.Validators;

namespace ForkPoint.Application.Tests.Validators;

// TestMethod_ExpectedResult_Scenario
public class RegisterUserValidatorTests
{
    private readonly RegisterUserValidator _validator = new();
    [Fact]
    public void RegisterRequest_FailsValidation_WhenEmailIsEmpty()
    {
        // Arrange
        var model = new RegisterRequest(string.Empty, "password123");

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void RegisterRequest_FailsValidation_WhenEmailIsInvalid()
    {
        // Arrange
        var model = new RegisterRequest("invalidemail", "password123");

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void RegisterRequest_PassesValidation_WhenEmailIsValid()
    {
        // Arrange
        var model = new RegisterRequest("test@example.com", "password123");

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void RegisterRequest_FailsValidation_WhenPasswordIsEmpty()
    {
        // Arrange
        var model = new RegisterRequest("test@example.com", string.Empty);

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void RegisterRequest_FailsValidation_WhenPasswordIsTooShort()
    {
        // Arrange
        var model = new RegisterRequest("test@example.com", "123");

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}
