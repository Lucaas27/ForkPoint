using FluentValidation;
using ForkPoint.Application.Models.Handlers.GetAllRestaurants;

namespace ForkPoint.Application.Validators;

public class GetAllRestaurantsRequestValidator : AbstractValidator<GetAllRestaurantsRequest>
{
    public GetAllRestaurantsRequestValidator()
    {
        RuleFor(x => x.Pagination).SetValidator(new PaginationValidator());
    }
}