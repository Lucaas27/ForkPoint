using FluentValidation;
using ForkPoint.Application.Enums;
using ForkPoint.Application.Models.Handlers.GetAllRestaurants;
using ForkPoint.Domain.Enums;

namespace ForkPoint.Application.Validators;

public class GetAllRestaurantsRequestValidator : AbstractValidator<GetAllRestaurantsRequest>
{
    private readonly int[] _allowedPageSizes = [.. Enum.GetValues(typeof(PageSizeOptions)).Cast<int>()];

    private readonly string[] _allowedSearchOptions = [.. Enum.GetNames(typeof(SearchOptions))];

    private readonly string[] _allowedSortByOptions = [.. Enum.GetNames(typeof(SortByOptions))];

    public GetAllRestaurantsRequestValidator()
    {
        // Must be greater than or equal to 1.
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        // Must be one of the allowed page sizes.
        RuleFor(x => x.PageSize)
            .Must(value => _allowedPageSizes.Contains(value))
            .WithMessage($"Page size must be one of [{string.Join(", ", _allowedPageSizes)}]");


        // Must be a valid enum value if provided.
        RuleFor(x => x.SortBy)
            .IsInEnum()
            .When(x => x.SortBy != null)
            .WithMessage($"Sort by must be one of [{string.Join(", ", _allowedSortByOptions)}]");

        // Must be a valid enum value if provided.
        RuleFor(x => x.SearchBy)
            .IsInEnum()
            .When(x => x.SearchBy != null)
            .WithMessage($"Search by must be one of [{string.Join(", ", _allowedSearchOptions)}]");

        // Custom rule: SortBy must be provided if SortDirection is provided.
        RuleFor(x => x)
            .Custom((request, context) =>
            {
                if (request.SortDirection != null && request.SortBy == null)
                {
                    context.AddFailure("SortBy",
                        $"A valid SortBy value must be provided when SortDirection is provided. SortBy must be one of [{string.Join(", ", _allowedSortByOptions)}]");
                }
            });

        // Custom rule: SearchBy must be provided if SearchTerm is provided.
        // Custom rule: SearchTerm must be provided if SearchBy is provided.
        RuleFor(x => x)
            .Custom((request, context) =>
            {
                if (request.SearchTerm != null && request.SearchBy == null)
                {
                    context.AddFailure("SearchBy",
                        $"SearchBy must be provided when SearchTerm is provided. SearchBy must be one of [{string.Join(", ", _allowedSearchOptions)}]");
                }

                if (request.SearchBy != null && request.SearchTerm == null)
                {
                    context.AddFailure("SearchTerm",
                        "SearchTerm must be provided when SearchBy is provided.");
                }
            });
    }
}