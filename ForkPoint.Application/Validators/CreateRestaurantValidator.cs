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
            .Length(3, 100)
            .WithMessage("Name must be between 3 and 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.Category)
            .Must(category => _categories
                .Any(enumOption => enumOption.Equals(category.Replace(" ", ""), StringComparison.OrdinalIgnoreCase)))
            .WithMessage($"Invalid category. Available categories: {string.Join(", ", _categories)}");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email address")
            .MaximumLength(100)
            .WithMessage("Email cannot exceed 100 characters");

        RuleFor(x => x.ContactNumber)
            .MaximumLength(20)
            .WithMessage("Contact number cannot exceed 20 characters")
            .Matches(@"^(\+?[1-9]\d{1,3}\s?)?(0?\d{10})$")
            .WithMessage("Invalid contact number format. Expected format: +44 7567890123 or 01234567890");

        RuleFor(x => x.Street)
            .MaximumLength(100)
            .WithMessage("Street cannot exceed 100 characters");

        RuleFor(x => x.City)
            .MaximumLength(100)
            .WithMessage("City cannot exceed 100 characters");

        RuleFor(x => x.County)
            .MaximumLength(100)
            .WithMessage("County cannot exceed 100 characters");

        RuleFor(x => x.PostCode)
            .MaximumLength(20)
            .WithMessage("PostCode cannot exceed 20 characters");

        RuleFor(x => x.Country)
            .MaximumLength(100)
            .WithMessage("Country cannot exceed 100 characters");
    }
}
