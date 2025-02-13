using AutoMapper;
using ForkPoint.Application.Contexts;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetCurrentUserRestaurants;
using ForkPoint.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class GetCurrentUserRestaurantsHandler(
        ILogger<GetCurrentUserRestaurantsHandler> logger,
        UserManager<User> userManager,
        IUserContext userContext,
        IMapper mapper
    )
    : IRequestHandler<GetCurrentUserRestaurantsRequest, GetCurrentUserRestaurantsResponse>
{
    public async Task<GetCurrentUserRestaurantsResponse> Handle(
        GetCurrentUserRestaurantsRequest request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetCurrentUser() ?? throw new InvalidOperationException("User not authenticated");

        logger.LogInformation("Getting restaurants owned by user {@User}...", user);

        var dbUser = await userManager.Users
                         .Include(u => u.OwnedRestaurants)
                         .FirstOrDefaultAsync(u => u.Email == user.Email, cancellationToken) ??
                     throw new InvalidOperationException("User not found in the database");

        var restaurants = dbUser.OwnedRestaurants;

        var restaurantsDto = mapper.Map<IEnumerable<RestaurantModel>>(restaurants).ToList();

        return new GetCurrentUserRestaurantsResponse(restaurantsDto, dbUser.Id)
        {
            IsSuccess = true
        };
    }
}