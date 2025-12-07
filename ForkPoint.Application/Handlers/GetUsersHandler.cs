using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetUsers;
using ForkPoint.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ForkPoint.Domain.Repositories;

namespace ForkPoint.Application.Handlers;

public class GetUsersHandler : IRequestHandler<GetUsersRequest, GetUsersResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        var page = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var size = request.PageSize <= 0 ? 10 : request.PageSize;

        var total = await _userRepository.CountAsync(cancellationToken);

        var users = await _userRepository.GetUsersPageAsync(page, size, cancellationToken);

        var items = new List<CurrentUserModel>();

        foreach (var u in users)
        {
            var roles = await _userRepository.GetRolesAsync(u, cancellationToken);
            var name = u.FullName ?? u.UserName ?? u.Email ?? string.Empty;
            items.Add(new CurrentUserModel(u.Id, u.Email ?? string.Empty, roles, name));
        }

        return new GetUsersResponse(items, total, page, size);
    }
}
