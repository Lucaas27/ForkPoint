using ForkPoint.Domain.Entities;

namespace ForkPoint.Domain.Repositories;
public interface IMenuRepository
{
    Task<int> CreateMenuItemAsync(MenuItem entity);
    Task DeleteMenuItemAsync(MenuItem entity);
    Task DeleteAllMenuItemsAsync(IEnumerable<MenuItem> entities);
}
