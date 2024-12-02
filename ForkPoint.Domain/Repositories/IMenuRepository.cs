using ForkPoint.Domain.Entities;

namespace ForkPoint.Domain.Repositories;
public interface IMenuRepository
{
    Task<int> CreateMenuItemAsync(MenuItem entity);
    Task UpdateDb();
    Task DeleteMenuItem(MenuItem menuItem);
}
