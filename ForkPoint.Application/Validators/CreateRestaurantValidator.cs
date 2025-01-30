// Ignore Spelling: Validator

using FluentValidation;
using ForkPoint.Application.Enums;
using ForkPoint.Application.Models.Handlers.CreateRestaurant;

namespace ForkPoint.Application.Validators;

public class CreateRestaurantValidator : AbstractValidator<CreateRestaurantRequest>
{
    private readonly string[] _categories = Enum.GetNames<RestaurantCategories>();

    public CreateRestaurantValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 100)
            .WithMessage("Name must be between 3 and 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(10, 500)
            .WithMessage("Description must be between 10 and 500 characters");

        RuleFor(x => x.Category)
            .Must(category => _categories
                .Any(enumOption => enumOption.Equals(category.Replace(" ", ""), StringComparison.OrdinalIgnoreCase)))
            .WithMessage(
                $"Invalid category. Available categories: {string.Join(", ", _categories)}");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email address")
            .Length(10, 50)
            .WithMessage("Email must be between 10 and 50 characters");

        When(x => x.ContactNumber != null, () =>
        {
            RuleFor(x => x.ContactNumber)
                .MaximumLength(13)
                .Matches(@"^(\+|0)[1-9]\d{1,3}\s?\d{10}$")
                .WithMessage("Invalid contact number format. Expected format: +441234567890 or 01234567890");
        });

        RuleFor(x => x.Address)
            .NotEmpty()
            .NotNull();

        When(x => x.Address != null, () =>
        {
            RuleFor(x => x.Address.Street)
                .NotEmpty()
                .Length(5, 100)
                .WithMessage("Street must be between 5 and 100 characters");

            RuleFor(x => x.Address.City)
                .NotEmpty()
                .Length(3, 50)
                .WithMessage("City must be between 3 and 50 characters");

            RuleFor(x => x.Address.County)
                .NotEmpty()
                .Length(3, 50)
                .WithMessage("County must be between 3 and 50 characters");

            RuleFor(x => x.Address.PostCode)
                .NotEmpty()
                .Length(6, 10)
                .WithMessage("PostCode must be between 6 and 10 characters");

            RuleFor(x => x.Address.Country)
                .NotEmpty()
                .Length(2, 50)
                .WithMessage("Country must be between 2 and 50 characters");
        });
    }
}