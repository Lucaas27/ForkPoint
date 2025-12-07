using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers;

namespace ForkPoint.Application.Models.Handlers.GetUsers;

public record GetUsersResponse : BasePaginatedResponse<CurrentUserModel>
{
    public GetUsersResponse(IEnumerable<CurrentUserModel> items, int totalItemsCount, int pageNumber, int pageSize)
        : base(items, totalItemsCount, pageNumber, pageSize)
    {
    }
}
