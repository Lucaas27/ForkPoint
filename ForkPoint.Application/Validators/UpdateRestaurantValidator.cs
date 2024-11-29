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
            .WithMessage("Invalid contact number format. Expected format: +44 7567890123 or 01234567890");
    }
}
