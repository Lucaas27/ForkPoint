using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetById;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

/// <summary>
/// Handles the request to get a restaurant by its ID.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="mapper">The mapper instance for mapping domain models to DTOs.</param>
/// <param name="restaurantsRepository">The repository instance for accessing restaurant data.</param>
public class GetByIdHandler(
    ILogger<GetByIdHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository)
    : BaseHandler<GetByIdRequest, GetByIdResponse>(logger, mapper, restaurantsRepository)
{
    /// <summary>
    /// Handles the request to get a restaurant by its ID.
    /// </summary>
    /// <param name="request">The request containing the ID of the restaurant to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A response containing the restaurant details if found.</returns>
    /// <exception cref="NotFoundException">Thrown when the restaurant with the specified ID is not found.</exception>
    public override async Task<GetByIdResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request: {@Request}", request);
        _logger.LogInformation("Getting restaurant by id...");

        var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Restaurant), request.Id);

        var restaurantDTO = _mapper.Map<RestaurantDetailsModel>(restaurant);

        return new GetByIdResponse
        {
            IsSuccess = restaurantDTO is not null,
            Restaurant = restaurantDTO
        };
    }
}
