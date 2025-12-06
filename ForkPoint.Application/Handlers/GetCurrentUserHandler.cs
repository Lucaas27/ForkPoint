using ForkPoint.Application.Contexts;
using ForkPoint.Application.Models.Handlers.GetCurrentUser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class GetCurrentUserHandler (
    ILogger<GetCurrentUserHandler> logger,
    IUserContext userContext
    ): IRequestHandler<GetCurrentUserRequest, GetCurrentUserResponse>
{
    public Task<GetCurrentUserResponse> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();

        if (user is null)
        {
            return Task.FromResult(new GetCurrentUserResponse(null)
            {
                IsSuccess = false,
                Message = "Not authenticated"
            });
        }

        logger.LogInformation("Retrieved current user: {UserId}", user.Id);

        var response = new GetCurrentUserResponse(user);
        return Task.FromResult(response);
    }
}
