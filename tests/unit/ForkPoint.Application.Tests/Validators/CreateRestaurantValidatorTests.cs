using FluentValidation.TestHelper;
using ForkPoint.Application.Enums;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.CreateRestaurant;
using ForkPoint.Application.Validators;

namespace ForkPoint.Application.Tests.Validators;

// TestMethod_ExpectedResult_Scenario
public class CreateRestaurantValidatorTests
{
    private readonly CreateRestaurantValidator _validator = new();

    [Fact]
    public void CreateRestaurantRequest_PassesValidation_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = new string('A', 3),
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenNameIsTooLong()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = new string('A', 101),
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name must be between 3 and 100 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Address);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenNameIsTooShort()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = new string('A', 2),
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            HasDelivery = true,
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name must be between 3 and 100 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Address);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenNameIsEmpty()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = new string(' ', 50),
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("'Name' must not be empty.");
        result.ShouldNotHaveValidationErrorFor(x => x.Address);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenNameIsNull()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("'Name' must not be empty.");
        result.ShouldNotHaveValidationErrorFor(x => x.Address);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenDescriptionIsTooLong()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = new string('A', 5),
            Description = new string('A', 501),
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            HasDelivery = true,
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description must be between 10 and 500 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Address);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenDescriptionIsTooShort()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = new string('A', 5),
            Description = new string('A', 9),
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            HasDelivery = true,
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description must be between 10 and 500 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Address);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);

    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenDescriptionIsEmpty()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = new string('A', 50),
            Description = new string(' ', 50),
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("'Description' must not be empty.");
        result.ShouldNotHaveValidationErrorFor(x => x.Address);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);

    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenDescriptionIsNull()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("'Description' must not be empty.");
        result.ShouldNotHaveValidationErrorFor(x => x.Address);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);

    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenCategoryIsInvalid()
    {
        // Arrange
        var categories = Enum.GetNames<RestaurantCategories>();
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Invalid",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Category)
            .WithErrorMessage($"Invalid category. Available categories: {string.Join(", ", categories)}");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.Address);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Theory]
    [InlineData("invalid.emailexample.com")]
    public void CreateRestaurantRequest_FailsValidation_WhenEmailIsInvalid(string email)
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = email,
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email address");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.Address);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenEmailIsTooShort()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "i@n.c",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email must be between 10 and 50 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.Address);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenEmailIsTooLong()
    {

        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "i@n.c" + new string('A', 51),
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email must be between 10 and 50 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.Address);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("123456789012345678901")]
    [InlineData("123-456-7890")]
    [InlineData("123.456.7890")]
    [InlineData("1234567890")]
    [InlineData("+1 1234567890")]
    [InlineData("01234567890")]
    [InlineData("Invalid")]
    public void CreateRestaurantRequest_FailsValidation_WhenContactNumberIsInvalid(string number)
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = number,
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ContactNumber)
            .WithErrorMessage("Invalid contact number format. Expected format: +441234567890 or 01234567890");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Address);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressIsNull()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address)
            .WithErrorMessage("'Address' must not be empty.");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }


    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressStreetIsTooShort()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "A",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.Street)
            .WithErrorMessage("Street must be between 5 and 100 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressStreetIsTooLong()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = new string('A', 101),
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.Street)
            .WithErrorMessage("Street must be between 5 and 100 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressStreetIsEmpty()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = new string(' ', 50),
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.Street)
            .WithErrorMessage("'Address Street' must not be empty.");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressCityIsNull()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.City)
            .WithErrorMessage("'Address City' must not be empty.");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }


    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressCityIsTooShort()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "A",
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.City)
            .WithErrorMessage("City must be between 3 and 50 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressCityIsTooLong()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = new string('A', 51),
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.City)
            .WithErrorMessage("City must be between 3 and 50 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressCityIsEmpty()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = new string(' ', 50),
                County = "Valid County",
                PostCode = "123456",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.City)
            .WithErrorMessage("'Address City' must not be empty.");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressCountyIsNull()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = new string('a', 50),
                City = "Valid City",
                PostCode = "123456",
                Country = "United Kingdom"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.County)
            .WithErrorMessage("'Address County' must not be empty.");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }


    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressCountyIsTooShort()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = new string('a', 50),
                City = "Valid City",
                County = new string('a', 2),
                PostCode = "123456",
                Country = "United Kingdom"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.County)
            .WithErrorMessage("County must be between 3 and 50 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressCountyIsTooLong()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = new string('a', 50),
                City = "Valid City",
                County = new string('a', 51),
                PostCode = "123456",
                Country = "United Kingdom"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.County)
            .WithErrorMessage("County must be between 3 and 50 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressCountyIsEmpty()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = new string('a', 50),
                City = "Valid City",
                County = " ",
                PostCode = "123456",
                Country = "United Kingdom"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.County)
            .WithErrorMessage("'Address County' must not be empty.");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressPostcodeIsNull()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.PostCode)
            .WithErrorMessage("'Address Post Code' must not be empty.");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }


    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressPostcodeIsTooShort()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "12345",
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.PostCode)
            .WithErrorMessage("PostCode must be between 6 and 10 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressPostcodeIsTooLong()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = new string('A', 11),
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.PostCode)
            .WithErrorMessage("PostCode must be between 6 and 10 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressPostcodeIsEmpty()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = new string(' ', 10),
                Country = "Valid Country"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.PostCode)
            .WithErrorMessage("'Address Post Code' must not be empty.");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }


    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressCountryIsNull()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.Country)
            .WithErrorMessage("'Address Country' must not be empty.");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }


    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressCountryIsTooShort()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = "A"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.Country)
            .WithErrorMessage("Country must be between 2 and 50 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressCountryIsTooLong()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = new string('A', 51)
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.Country)
            .WithErrorMessage("Country must be between 2 and 50 characters");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }

    [Fact]
    public void CreateRestaurantRequest_FailsValidation_WhenAddressCountryIsEmpty()
    {
        // Arrange
        var request = new CreateRestaurantRequest
        {
            Name = "Valid Name",
            Description = "Valid Description",
            Category = "Cafe",
            Email = "valid.email@example.com",
            ContactNumber = "+441234567890",
            Address = new AddressModel
            {
                Street = "Valid Street",
                City = "Valid City",
                County = "Valid County",
                PostCode = "123456",
                Country = new string(' ', 50)
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Address.Country)
            .WithErrorMessage("'Address Country' must not be empty.");
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Category);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.HasDelivery);
    }





}
