using AutoMapper;
using ForkPoint.Application.Models.Handlers.DeleteRestaurant;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;
public class DeleteRestaurantHandler(
    ILogger<DeleteRestaurantHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository)
    : BaseHandler<DeleteRestaurantRequest, DeleteRestaurantResponse>(logger, mapper, restaurantsRepository)
{
    public override async Task<DeleteRestaurantResponse> Handle(DeleteRestaurantRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Deleting restaurant id: {request.Id}...");

        var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id);

        if (restaurant is null)
        {
            _logger.LogError($"Restaurant with id {request.Id} not found.");
            return new DeleteRestaurantResponse
            {
                IsSuccess = false,
                Message = $"Restaurant with id {request.Id} not found."
            };
        }

        await _restaurantsRepository.Delete(restaurant);

        _logger.LogInformation($"Restaurant with id {request.Id} deleted.");

        return new DeleteRestaurantResponse
        {
            IsSuccess = true,
            Message = $"Restaurant with id {request.Id} deleted."
        };
    }
}
