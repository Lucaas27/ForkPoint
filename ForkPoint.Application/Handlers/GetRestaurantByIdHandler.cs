using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetRestaurantById;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

/// <summary>
///     Handles the request to get a restaurant by its ID.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="mapper">The mapper instance for mapping domain models to DTOs.</param>
/// <param name="restaurantsRepository">The repository instance for accessing restaurant data.</param>
public class GetRestaurantByIdHandler(
    ILogger<GetRestaurantByIdHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository)
    : BaseHandler<GetRestaurantByIdRequest, GetRestaurantByIdResponse>
{
    /// <summary>
    ///     Handles the request to get a restaurant by its ID.
    /// </summary>
    /// <param name="request">The request containing the ID of the restaurant to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A response containing the restaurant details if found.</returns>
    /// <exception cref="NotFoundException">Thrown when the restaurant with the specified ID is not found.</exception>
    public override async Task<GetRestaurantByIdResponse> Handle(GetRestaurantByIdRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Request: {@Request}", request);
        logger.LogInformation("Getting restaurant by id {@RestaurantId}...", request.Id);

        var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.Id)
                         ?? throw new NotFoundException(nameof(Restaurant), request.Id);

        var restaurantDto = mapper.Map<RestaurantDetailsModel>(restaurant);

        return new GetRestaurantByIdResponse(restaurantDto)
        {
            IsSuccess = true
        };
    }
}