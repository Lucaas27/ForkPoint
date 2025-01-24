using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetAllRestaurants;
using ForkPoint.Domain.Models;
using ForkPoint.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

/// <summary>
///     Handles the request to get all restaurants.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="mapper">The AutoMapper instance.</param>
/// <param name="restaurantsRepository">The repository for accessing restaurant data.</param>
public class GetAllRestaurantsHandler(
    ILogger<GetAllRestaurantsHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository
)
    : IRequestHandler<GetAllRestaurantsRequest, GetAllRestaurantsResponse>
{
    /// <summary>
    ///     Handles the request to get all restaurants.
    /// </summary>
    /// <param name="request">The request to get all restaurants.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A response containing all restaurants.</returns>
    public async Task<GetAllRestaurantsResponse> Handle(
        GetAllRestaurantsRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Getting all restaurants...");

        var filterOptions = new RestaurantFilterOptions
        {
            SearchBy = request.SearchBy,
            SearchTerm = request.SearchTerm,
            PageNumber = request.PageNumber,
            PageSize = (int)request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection
        };

        var (restaurants, totalCount) = await restaurantsRepository.GetFilteredRestaurantsAsync(filterOptions);

        var restaurantsDto = mapper.Map<IEnumerable<RestaurantModel>>(restaurants);

        var response =
            new GetAllRestaurantsResponse(restaurantsDto, totalCount, request.PageNumber,
                (int)request.PageSize)
            {
                IsSuccess = true
            };

        return response;
    }
}