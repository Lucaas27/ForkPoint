using FluentAssertions;
using FluentValidation.TestHelper;
using ForkPoint.Application.Models.Handlers.EmailConfirmation;
using ForkPoint.Application.Validators;

namespace ForkPoint.Application.Tests.Validators;

// TestMethod_ExpectedResult_Scenario

public class ConfirmEmailRequestValidatorTests
{
    private readonly ConfirmEmailRequestValidator _validator = new();

    [Fact()]
    public void ConfirmEmailRequestRequest_PassesValidation_WhenRequestIsValid()
    {

        // Assert
        var request = new ConfirmEmailRequest("123456", "test@test.com");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveAnyValidationErrors();

    }

    [Fact]
    public void ConfirmEmailRequestRequest_FailsValidation_WhenEmailIsInvalid()
    {
        // Arrange
        var request = new ConfirmEmailRequest("123456", "wrongemailformat");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void ConfirmEmailRequestRequest_FailsValidation_WhenEmailIsEmpty()
    {
        // Arrange
        var request = new ConfirmEmailRequest("123456", " ");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void ConfirmEmailRequestRequest_FailsValidation_WhenTokenIsEmpty()
    {
        // Arrange
        var request = new ConfirmEmailRequest(" ", "test@test.com");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Token);
    }
}