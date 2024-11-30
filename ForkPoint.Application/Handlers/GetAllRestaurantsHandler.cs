using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetAllRestaurants;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

/// <summary>
/// Handles the request to get all restaurants.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="mapper">The AutoMapper instance.</param>
/// <param name="restaurantsRepository">The repository for accessing restaurant data.</param>
public class GetAllRestaurantsHandler(
    ILogger<GetAllRestaurantsHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository)
    : BaseHandler<GetAllRestaurantsRequest, GetAllRestaurantsResponse>(logger, mapper, restaurantsRepository)
{
    /// <summary>
    /// Handles the request to get all restaurants.
    /// </summary>
    /// <param name="request">The request to get all restaurants.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A response containing all restaurants.</returns>
    public override async Task<GetAllRestaurantsResponse> Handle(GetAllRestaurantsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request: {@Request}", request);
        _logger.LogInformation("Getting all restaurants...");

        var restaurants = await _restaurantsRepository!.GetAllRestaurantsAsync();

        var response = new GetAllRestaurantsResponse
        {
            IsSuccess = restaurants.Any(),
            Restaurants = _mapper.Map<IEnumerable<RestaurantModel>>(restaurants)
        };

        return response;
    }
}
