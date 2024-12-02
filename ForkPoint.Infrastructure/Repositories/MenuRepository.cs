using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using ForkPoint.Infrastructure.Persistence;

namespace ForkPoint.Infrastructure.Repositories;
internal class MenuRepository(ForkPointDbContext dbContext)
    : IMenuRepository
{
    public async Task<int> CreateMenuItemAsync(MenuItem entity)
    {
        var menuItem = dbContext.MenuItems.Add(entity);

        await UpdateDb();

        return menuItem.Entity.Id;
    }

    public Task DeleteMenuItem(MenuItem menuItem)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateDb()
    {
        await dbContext.SaveChangesAsync();
    }
}
