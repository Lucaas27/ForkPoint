using AutoMapper;
using ForkPoint.Application.ExternalClients.Foursquare;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetExternalRestaurants;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class GetExternalRestaurantsHandler(
    ILogger<GetExternalRestaurantsHandler> logger,
    IMapper mapper,
    IFoursquareClient foursquareClient
) : IRequestHandler<GetExternalRestaurantsRequest, GetExternalRestaurantsResponse>
{
    public async Task<GetExternalRestaurantsResponse> Handle(
        GetExternalRestaurantsRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Handling GetExternalRestaurantsRequest");

        // Call the api
        var fsResponse = await foursquareClient.SearchAsync(cancellationToken);

        var places = fsResponse.Results ?? Enumerable.Empty<Place>();
        var items = mapper.Map<IEnumerable<RestaurantModel>>(places).ToList();

        var totalItems = items.Count;
        var pageSize = (int)request.PageSize;
        var pageNumber = Math.Max(1, request.PageNumber);

        var pagedItems = items
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var response =
            new GetExternalRestaurantsResponse(pagedItems, totalItems, pageNumber, pageSize)
            {
                IsSuccess = true
            };

        logger.LogInformation("GetExternalRestaurantsRequest handled successfully");

        return response;
    }
}
