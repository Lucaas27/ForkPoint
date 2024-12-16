// Ignore Spelling: Validator

using FluentValidation;
using ForkPoint.Application.Models.Handlers.UpdateRestaurant;

namespace ForkPoint.Application.Validators;

public class UpdateRestaurantValidator : AbstractValidator<UpdateRestaurantRequest>
{
    public UpdateRestaurantValidator()
    {
        RuleFor(x => x.Name)
            .Length(3, 100)
            .WithMessage("Name must be between 3 and 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email address")
            .MaximumLength(100)
            .WithMessage("Email cannot exceed 100 characters");

        RuleFor(x => x.ContactNumber)
            .MaximumLength(20)
            .WithMessage("Contact number cannot exceed 20 characters")
            .Matches(@"^(\+?[1-9]\d{1,3}\s?)?(0?\d{10})$")
            .WithMessage("Invalid contact number format. Expected format: +447567890123 or 01234567890");

        When(x => x.Address is not null, () =>
        {
            RuleFor(x => x.Address!.Street)
                .Length(5, 100)
                .WithMessage("Street must be between 5 and 100 characters");

            RuleFor(x => x.Address!.City)
                .Length(3, 50)
                .WithMessage("City must be between 3 and 50 characters");

            RuleFor(x => x.Address!.County)
                .Length(3, 50)
                .WithMessage("County must be between 3 and 50 characters");

            RuleFor(x => x.Address!.PostCode)
                .Length(6, 10)
                .WithMessage("PostCode must be between 6 and 10 characters");

            RuleFor(x => x.Address!.Country)
                .Length(2, 50)
                .WithMessage("Country must be between 2 and 50 characters");
        });
    }
}