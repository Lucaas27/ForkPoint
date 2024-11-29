using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetAll;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;
public class GetAllHandler(
    ILogger<GetAllHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository)
    : BaseHandler<GetAllRequest, GetAllResponse>(logger, mapper, restaurantsRepository)
{

    public override async Task<GetAllResponse> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request: {@Request}", request);
        _logger.LogInformation("Getting all restaurants...");

        var restaurants = await _restaurantsRepository.GetAllAsync();

        var response = new GetAllResponse
        {
            IsSuccess = restaurants.Any(),
            Restaurants = _mapper.Map<IEnumerable<RestaurantModel>>(restaurants)
        };

        return response;
    }
}
