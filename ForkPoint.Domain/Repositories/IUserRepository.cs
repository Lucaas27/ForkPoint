using ForkPoint.Domain.Entities;

namespace ForkPoint.Domain.Repositories;

public interface IUserRepository
{
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<List<User>> GetUsersPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default);
}
