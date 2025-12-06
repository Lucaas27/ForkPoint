using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetCurrentUser;

public record GetCurrentUserResponse : BaseResponse
{
    public CurrentUserModel? User { get; init; }

    public GetCurrentUserResponse(CurrentUserModel? user)
    {
        User = user;
        IsSuccess = user is not null;
    }
}
