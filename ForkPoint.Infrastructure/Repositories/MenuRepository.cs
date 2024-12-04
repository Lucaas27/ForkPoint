using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using ForkPoint.Infrastructure.Persistence;

namespace ForkPoint.Infrastructure.Repositories;
internal class MenuRepository(ForkPointDbContext dbContext)
    : IMenuRepository
{
    public async Task<int> CreateMenuItemAsync(MenuItem entity)
    {
        var menuItem = await dbContext.MenuItems.AddAsync(entity);

        await UpdateDb();

        return menuItem.Entity.Id;
    }

    public async Task DeleteMenuItemAsync(MenuItem entity)
    {
        dbContext.MenuItems.Remove(entity);

        await UpdateDb();
    }

    public async Task DeleteAllMenuItemsAsync(IEnumerable<MenuItem> entities)
    {
        dbContext.MenuItems.RemoveRange(entities);

        await UpdateDb();
    }

    private async Task UpdateDb()
    {
        await dbContext.SaveChangesAsync();
    }
}
