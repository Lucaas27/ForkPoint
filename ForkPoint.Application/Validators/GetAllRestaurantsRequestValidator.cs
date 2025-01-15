using FluentValidation;
using ForkPoint.Application.Models.Handlers.GetAllRestaurants;

namespace ForkPoint.Application.Validators;

public class GetAllRestaurantsRequestValidator : AbstractValidator<GetAllRestaurantsRequest>
{
    private readonly int[] _allowedPageSizes = [10, 20, 30, 40, 50];

    public GetAllRestaurantsRequestValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(value => _allowedPageSizes.Contains(value))
            .WithMessage($"Page size must be one of [{string.Join(',', _allowedPageSizes)}]");
    }
}