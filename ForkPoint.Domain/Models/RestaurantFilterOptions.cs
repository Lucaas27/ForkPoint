using ForkPoint.Domain.Enums;

namespace ForkPoint.Domain.Models;

public record RestaurantFilterOptions
{
    public SearchOptions? SearchBy { get; init; }
    public string? SearchTerm { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public SortByOptions? SortBy { get; init; }
    public SortDirection? SortDirection { get; init; }
}