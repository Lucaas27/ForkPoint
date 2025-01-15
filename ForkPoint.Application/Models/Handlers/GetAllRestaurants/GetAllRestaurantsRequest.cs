using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetAllRestaurants;

public record GetAllRestaurantsRequest(string? SearchTerm, int PageNumber, int PageSize)
    : IRequest<GetAllRestaurantsResponse>;