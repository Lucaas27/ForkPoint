using FluentValidation;
using ForkPoint.Application.Models.Handlers;

namespace ForkPoint.Application.Validators;

public class PaginationValidator : AbstractValidator<Pagination>
{
    private readonly int[] _allowedPageSizes = [10, 20, 30, 40, 50];

    public PaginationValidator()
    {
        RuleFor(p => p.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(p => p.PageSize)
            .Must(value => _allowedPageSizes.Contains(value))
            .WithMessage($"Page size must be one of [{string.Join(',', _allowedPageSizes)}]");
    }
}