using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetAll;
using ForkPoint.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;
public class GetAllHandler(ILogger<GetByIdHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository)
    : IRequestHandler<GetAllRequest, GetAllResponse>
{

    public async Task<GetAllResponse> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants...");

        var restaurants = await restaurantsRepository.GetAllAsync();

        var response = new GetAllResponse
        {
            IsSuccess = restaurants.Any(),
            Restaurants = mapper.Map<IEnumerable<RestaurantModel>>(restaurants)
        };

        return response;
    }
}
