using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetById;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;
public class GetByIdHandler(
    ILogger<GetByIdHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository)
    : BaseHandler<GetByIdRequest, GetByIdResponse>(logger, mapper, restaurantsRepository)
{
    public override async Task<GetByIdResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request: {@Request}", request);
        _logger.LogInformation("Getting restaurant by id...");

        var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id);

        var restaurantDTO = _mapper.Map<RestaurantDetailsModel?>(restaurant);

        return new GetByIdResponse
        {
            IsSuccess = restaurantDTO is not null,
            Restaurant = restaurantDTO
        };



    }
}
