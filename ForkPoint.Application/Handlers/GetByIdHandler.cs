using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetById;
using ForkPoint.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;
public class GetByIdHandler(ILogger<GetByIdHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository) : IRequestHandler<GetByIdRequest, GetByIdResponse>
{
    public async Task<GetByIdResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting restaurant by id: {request.Id}");

        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);

        var restaurantDTO = mapper.Map<RestaurantDetailsModel?>(restaurant);

        return new GetByIdResponse
        {
            IsSuccess = restaurantDTO is not null,
            Restaurant = restaurantDTO
        };



    }
}
