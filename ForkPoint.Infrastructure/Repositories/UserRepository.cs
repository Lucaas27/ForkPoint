using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ForkPoint.Infrastructure.Repositories;

internal class UserRepository(UserManager<User> userManager) : IUserRepository
{
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await userManager.Users.CountAsync(cancellationToken);
    }

    public async Task<List<User>> GetUsersPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var page = pageNumber <= 0 ? 1 : pageNumber;
        var size = pageSize <= 0 ? 10 : pageSize;

        var users = await userManager.Users
            .OrderBy(u => u.Id)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return users;
    }

    public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default)
    {
        return await userManager.GetRolesAsync(user);
    }
}
