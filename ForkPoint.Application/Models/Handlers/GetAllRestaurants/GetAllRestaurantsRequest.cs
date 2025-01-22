using ForkPoint.Domain.Enums;
using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetAllRestaurants;

public record GetAllRestaurantsRequest(
    SearchOptions? SearchBy,
    string? SearchTerm,
    int PageNumber,
    PageSizeOptions PageSize,
    SortByOptions? SortBy,
    SortDirection? SortDirection
)
    : IRequest<GetAllRestaurantsResponse>;