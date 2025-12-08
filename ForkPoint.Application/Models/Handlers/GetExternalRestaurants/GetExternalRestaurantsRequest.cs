using ForkPoint.Domain.Enums;
using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetExternalRestaurants;

public record GetExternalRestaurantsRequest(
    int PageNumber,
    PageSizeOptions PageSize
) : IRequest<GetExternalRestaurantsResponse>;
