using ForkPoint.Domain.Entities;

namespace ForkPoint.Domain.Repositories;
public interface IMenuRepository
{
    Task<IEnumerable<MenuItem>?> GetMenuAsync(int restaurantId);
    Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId);
    Task<int> CreateMenuItemAsync(MenuItem entity);
    Task UpdateDb();
    Task DeleteMenuItem(MenuItem menuItem);
}
