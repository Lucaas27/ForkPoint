﻿// Ignore Spelling: Validator

using FluentValidation;
using ForkPoint.Application.Models.Handlers.CreateMenuItem;

namespace ForkPoint.Application.Validators;
public class CreateMenuItemValidator : AbstractValidator<CreateMenuItemRequest>
{
    public CreateMenuItemValidator()
    {
        RuleFor(x => x.Name)
            .Length(3, 100)
            .WithMessage("Name must be between 3 and 50 characters");

        RuleFor(x => x.Description)
            .Length(3, 100)
            .WithMessage("Description must be between 3 and 50 characters");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price cannot be a negative number.");

        RuleFor(x => x.ImageUrl)
            .Length(5, 100)
            .WithMessage("ImageUrl must be between 5 and 50 characters");

        RuleFor(x => x.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("KiloCalories cannot be a negative number.");
    }
}