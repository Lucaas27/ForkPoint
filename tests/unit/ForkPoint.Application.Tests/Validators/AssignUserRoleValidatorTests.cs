using FluentAssertions;
using FluentValidation.TestHelper;
using ForkPoint.Application.Models.Handlers.AssignUserRole;
using ForkPoint.Application.Validators;

namespace ForkPoint.Application.Tests.Validators;

// TestMethod_ExpectedResult_Scenario
public class AssignUserRoleValidatorTests
{
    private readonly AssignUserRoleValidator _validator = new();

    [Theory]
    [InlineData("Admin")]
    [InlineData("admin")]
    [InlineData("User")]
    [InlineData("user")]
    [InlineData("Owner")]
    [InlineData("owner")]
    public void AssignUserRoleRequest_PassesValidation_WhenRequestIsValid(string role)
    {
        // Arrange
        var request = new AssignUserRoleRequest("test@test.com", role);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void AssignUserRoleRequest_FailsValidation_WhenEmailIsInvalid()
    {
        // Arrange
        var request = new AssignUserRoleRequest("testtest.com", "Admin");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void AssignUserRoleRequest_FailsValidation_WhenRoleIsInvalid()
    {
        // Arrange
        var request = new AssignUserRoleRequest("test@test.com", "test");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    [Fact]
    public void AssignUserRoleRequest_FailsValidation_WhenRoleIsEmpty()
    {
        // Arrange
        var request = new AssignUserRoleRequest("test@test.com", " ");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }


    [Fact]
    public void AssignUserRoleRequest_FailsValidation_WhenEmailIsEmpty()
    {
        // Arrange
        var request = new AssignUserRoleRequest(" ", "Admin");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}
