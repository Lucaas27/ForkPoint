using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using ForkPoint.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ForkPoint.Infrastructure.Repositories;
internal class MenuRepository(ForkPointDbContext dbContext)
    : IMenuRepository
{
    public Task<int> CreateMenuItemAsync(MenuItem entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteMenuItem(MenuItem menuItem)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<MenuItem>?> GetMenuAsync(int restaurantId)
    {
        var menu = await dbContext.MenuItems.Where(m => m.RestaurantId == restaurantId).ToListAsync();

        return menu;
    }

    public Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateDb()
    {
        throw new NotImplementedException();
    }
}
