using FluentAssertions;
using FluentValidation.TestHelper;
using ForkPoint.Application.Models.Handlers.CreateMenuItem;
using ForkPoint.Application.Validators;

namespace ForkPoint.Application.Tests.Validators;

// TestMethod_ExpectedResult_Scenario
public class CreateMenuItemValidatorTests
{
    private readonly CreateMenuItemValidator _validator = new();


    [Fact]
    public void CreateMenuItemRequest_PassesValidation_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateMenuItemRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Price = 10.0m,
            ImageUrl = "https://validurl.com/image.jpg",
            KiloCalories = 100
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateMenuItemRequest_FailsValidation_WhenNameIsNull()
    {
        // Arrange
        var request = new CreateMenuItemRequest
        {
            Name = null!,
            Description = "Valid Description",
            Price = 10.0m,
            ImageUrl = "https://validurl.com/image.jpg",
            KiloCalories = 100
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateMenuItemRequest_FailsValidation_WhenNameIsTooLong()
    {
        // Arrange
        var request = new CreateMenuItemRequest
        {
            Name = new string('a', 101),
            Description = "Valid Description",
            Price = 10.0m,
            ImageUrl = "https://validurl.com/image.jpg",
            KiloCalories = 100
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name must be between 3 and 100 characters");
    }

    [Fact]
    public void CreateMenuItemRequest_FailsValidation_WhenNameIsTooShort()
    {
        // Arrange
        var request = new CreateMenuItemRequest
        {
            Name = new string('a', 2),
            Description = "Valid Description",
            Price = 10.0m,
            ImageUrl = "https://validurl.com/image.jpg",
            KiloCalories = 100
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateMenuItemRequest_FailsValidation_WhenDescriptionIsNull()
    {
        // Arrange
        var request = new CreateMenuItemRequest
        {
            Name = "Valid Name",
            Description = null!,
            Price = 10.0m,
            ImageUrl = "https://validurl.com/image.jpg",
            KiloCalories = 100
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }


    [Fact]
    public void CreateMenuItemRequest_FailsValidation_WhenDescriptionIsTooLong()
    {
        // Arrange
        var request = new CreateMenuItemRequest
        {
            Name = "Valid Name",
            Description = new string('a', 101),
            Price = 10.0m,
            ImageUrl = "https://validurl.com/image.jpg",
            KiloCalories = 100
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description must be between 3 and 100 characters");
    }

    [Fact]
    public void CreateMenuItemRequest_FailsValidation_WhenDescriptionIsTooShort()
    {
        // Arrange
        var request = new CreateMenuItemRequest
        {
            Name = "Valid Name",
            Description = new string('a', 2),
            Price = 10.0m,
            ImageUrl = "https://validurl.com/image.jpg",
            KiloCalories = 100
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description must be between 3 and 100 characters");
    }


    [Fact]
    public void CreateMenuItemRequest_FailsValidation_WhenPriceIsNegative()
    {
        // Arrange
        var request = new CreateMenuItemRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Price = -1.0m,
            ImageUrl = "https://validurl.com/image.jpg",
            KiloCalories = 100
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Price)
            .WithErrorMessage("Price cannot be zero or a negative number.");
    }

    [Fact]
    public void CreateMenuItemRequest_FailsValidation_WhenPriceIsZero()
    {
        // Arrange
        var request = new CreateMenuItemRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Price = 0,
            ImageUrl = "https://validurl.com/image.jpg",
            KiloCalories = 100
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Price)
            .WithErrorMessage("Price cannot be zero or a negative number.");
    }

    [Fact]
    public void CreateMenuItemRequest_FailsValidation_WhenImageUrlIsTooShort()
    {
        // Arrange
        var request = new CreateMenuItemRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Price = 10.0m,
            ImageUrl = "abc",
            KiloCalories = 100
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.ImageUrl)
            .WithErrorMessage("ImageUrl must be between 5 and 100 characters");
    }

    [Fact]
    public void CreateMenuItemRequest_FailsValidation_WhenImageUrlIsTooLong()
    {
        // Arrange
        var request = new CreateMenuItemRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Price = 10.0m,
            ImageUrl = new string('a', 101),
            KiloCalories = 100
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.ImageUrl)
            .WithErrorMessage("ImageUrl must be between 5 and 100 characters");
    }

    [Fact]
    public void CreateMenuItemRequest_FailsValidation_WhenKiloCaloriesIsNegative()
    {
        // Arrange
        var request = new CreateMenuItemRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Price = 10.0m,
            ImageUrl = "https://validurl.com/image.jpg",
            KiloCalories = -10
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.KiloCalories)
            .WithErrorMessage("KiloCalories cannot be a negative number.");
    }

    [Fact]
    public void CreateMenuItemRequest_PassesValidation_WhenKiloCaloriesIsZero()
    {
        // Arrange
        var request = new CreateMenuItemRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Price = 10.0m,
            ImageUrl = "https://validurl.com/image.jpg",
            KiloCalories = 0
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveAnyValidationErrors();
    }
}