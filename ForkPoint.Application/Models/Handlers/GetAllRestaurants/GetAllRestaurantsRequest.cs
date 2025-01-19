using System.Text.Json.Serialization;
using ForkPoint.Domain.Enums;
using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetAllRestaurants;

public record GetAllRestaurantsRequest(
    SearchOptions? SearchBy,
    string? SearchTerm,
    int PageNumber,
    int PageSize,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    SortByOptions? SortBy,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    SortDirection? SortDirection
)
    : IRequest<GetAllRestaurantsResponse>;