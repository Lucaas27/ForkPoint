using FluentValidation.TestHelper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.UpdateRestaurant;
using ForkPoint.Application.Validators;

namespace ForkPoint.Application.Tests.Validators;

public class UpdateRestaurantValidatorTests
{
    private readonly UpdateRestaurantValidator _validator = new();
    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenNameIsTooShort()
    {
        var model = new UpdateRestaurantRequest { Name = "AB" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenNameIsTooLong()
    {
        var model = new UpdateRestaurantRequest { Name = new string('A', 101) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void UpdateRestaurantRequest_PassesValidation__WhenNameIsValid()
    {
        var model = new UpdateRestaurantRequest { Name = "Valid Name" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenDescriptionIsTooLong()
    {
        var model = new UpdateRestaurantRequest { Description = new string('A', 501) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void UpdateRestaurantRequest_PassesValidation__WhenDescriptionIsValid()
    {
        var model = new UpdateRestaurantRequest { Description = "Valid Description" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenEmailIsInvalid()
    {
        var model = new UpdateRestaurantRequest { Email = "invalid-email" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenEmailIsTooLong()
    {
        var model = new UpdateRestaurantRequest { Email = new string('A', 101) + "@example.com" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void UpdateRestaurantRequest_PassesValidation_WhenEmailIsValid()
    {
        var model = new UpdateRestaurantRequest { Email = "valid@example.com" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_ContactNumberIsInvalid()
    {
        var model = new UpdateRestaurantRequest { ContactNumber = "invalid-number" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ContactNumber);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_ContactNumberIsTooLong()
    {
        var model = new UpdateRestaurantRequest { ContactNumber = new string('1', 21) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ContactNumber);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenContactNumberIsValid()
    {
        var model = new UpdateRestaurantRequest { ContactNumber = "+447567890123" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.ContactNumber);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenAddressStreetIsTooShort()
    {
        var model = new UpdateRestaurantRequest
        {
            Address = new AddressModel { Street = "1234" }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Address!.Street);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenAddressStreetIsTooLong()
    {
        var model = new UpdateRestaurantRequest { Address = new AddressModel { Street = new string('A', 101) } };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Address!.Street);
    }

    [Fact]
    public void UpdateRestaurantRequest_PassValidation_WhenAddressStreetIsValid()
    {
        var model = new UpdateRestaurantRequest { Address = new AddressModel { Street = "Valid Street" } };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Address!.Street);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenAddressCityIsTooShort()
    {
        var model = new UpdateRestaurantRequest { Address = new AddressModel { City = "A" } };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Address!.City);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenAddressCityIsTooLong()
    {
        var model = new UpdateRestaurantRequest { Address = new AddressModel { City = new string('A', 51) } };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Address!.City);
    }

    [Fact]
    public void UpdateRestaurantRequest_PassesValidation_WhenAddressCityIsValid()
    {
        var model = new UpdateRestaurantRequest { Address = new AddressModel { City = "Valid City" } };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Address!.City);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenAddressCountyIsTooShort()
    {
        var model = new UpdateRestaurantRequest { Address = new AddressModel { County = "A" } };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Address!.County);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_When_Address_County_Is_Too_Long()
    {
        var model = new UpdateRestaurantRequest { Address = new AddressModel { County = new string('A', 51) } };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Address!.County);
    }

    [Fact]
    public void UpdateRestaurantRequest_PassValidation_WhenAddressCountyIsValid()
    {
        var model = new UpdateRestaurantRequest { Address = new AddressModel { County = "Valid County" } };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Address!.County);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenAddressPostCodeIsTooShort()
    {
        var model = new UpdateRestaurantRequest { Address = new AddressModel { PostCode = "12345" } };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Address!.PostCode);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenAddressPostCodeIsTooLong()
    {
        var model = new UpdateRestaurantRequest { Address = new AddressModel { PostCode = new string('A', 11) } };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Address!.PostCode);
    }

    [Fact]
    public void UpdateRestaurantRequest_PassValidation_WhenAddressPostCodeIsValid()
    {
        var model = new UpdateRestaurantRequest { Address = new AddressModel { PostCode = "123456" } };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Address!.PostCode);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenAddressCountryIsToosShort()
    {
        var model = new UpdateRestaurantRequest { Address = new AddressModel { Country = "A" } };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Address!.Country);
    }

    [Fact]
    public void UpdateRestaurantRequest_FailsValidation_WhenAddressCountryIsTooLong()
    {
        var model = new UpdateRestaurantRequest { Address = new AddressModel { Country = new string('A', 51) } };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Address!.Country);
    }

    [Fact]
    public void UpdateRestaurantRequest_PassValidation_WhenAddressCountryIsValid()
    {
        var model = new UpdateRestaurantRequest { Address = new AddressModel { Country = "Valid Country" } };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Address!.Country);
    }
}
