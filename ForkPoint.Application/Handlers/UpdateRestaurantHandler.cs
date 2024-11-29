using AutoMapper;
using ForkPoint.Application.Models.Handlers.UpdateRestaurant;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;
public class UpdateRestaurantHandler(
    ILogger<UpdateRestaurantHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantRepository)
    : BaseHandler<UpdateRestaurantRequest, UpdateRestaurantResponse>(logger, mapper, restaurantRepository)
{
    public override async Task<UpdateRestaurantResponse> Handle(UpdateRestaurantRequest request, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id);
        if (restaurant is null)
        {
            _logger.LogError($"Restaurant with id {request.Id} not found.");
            return new UpdateRestaurantResponse
            {
                IsSuccess = false,
                Message = $"Restaurant with id {request.Id} not found."
            };
        }

        _mapper.Map(request, restaurant);

        await _restaurantsRepository.UpdateDb();

        return new UpdateRestaurantResponse
        {
            IsSuccess = true,
            Message = "Restaurant updated successfully.",
        };
    }
}
