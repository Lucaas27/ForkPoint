using System.Linq.Expressions;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Enums;
using ForkPoint.Domain.Models;
using ForkPoint.Domain.Repositories;
using ForkPoint.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ForkPoint.Infrastructure.Repositories;

internal class RestaurantRepository(ApplicationDbContext dbContext)
    : IRestaurantRepository
{
    public async Task<int> CreateRestaurantAsync(Restaurant entity)
    {
        await dbContext.Restaurants.AddAsync(entity);
        await UpdateDb();

        return entity.Id;
    }

    public async Task DeleteRestaurant(Restaurant restaurant)
    {
        dbContext.Restaurants.Remove(restaurant);
        await UpdateDb();
    }

    public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
    {
        var restaurants = await dbContext.Restaurants.ToListAsync();
        return restaurants;
    }

    public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
    {
        var restaurant = await dbContext.Restaurants
            .Include(r => r.Address)
            .Include(r => r.MenuItems)
            .FirstOrDefaultAsync(r => r.Id == id);

        return restaurant;
    }

    public async Task UpdateDb()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Restaurant>, int)> GetFilteredRestaurantsAsync(RestaurantFilterOptions filterOptions)
    {
        var lowerCaseSearchTerm = filterOptions.SearchTerm?.ToLower();
        var query = dbContext.Restaurants.AsQueryable();


        if (filterOptions.SearchBy != null && !string.IsNullOrEmpty(lowerCaseSearchTerm))
        {
            var searchColumns = new Dictionary<SearchOptions, Expression<Func<Restaurant, bool>>>
            {
                { SearchOptions.Name, r => r.Name.ToLower().Contains(lowerCaseSearchTerm) },
                { SearchOptions.Category, r => r.Category.ToLower().Contains(lowerCaseSearchTerm) },
                { SearchOptions.Description, r => r.Description.ToLower().Contains(lowerCaseSearchTerm) }
            };

            var selectedColumn = searchColumns[filterOptions.SearchBy.Value];

            query = query.Where(selectedColumn);
        }

        if (filterOptions.SortBy != null)
        {
            var sortColumns = new Dictionary<SortByOptions, Expression<Func<Restaurant, string>>>
            {
                { SortByOptions.Name, r => r.Name.ToLower() },
                { SortByOptions.Category, r => r.Category.ToLower() },
                { SortByOptions.Description, r => r.Description.ToLower() }
            };

            var selectedColumn = sortColumns[filterOptions.SortBy.Value];

            query = filterOptions.SortDirection == SortDirection.Descending
                ? query.OrderByDescending(selectedColumn)
                : query.OrderBy(selectedColumn);
        }

        var totalCount = await query.CountAsync();

        // For pagination: PageSize = 5, PageNumber = 2
        // Skip = PageSize * (PageNumber - 1) = 5
        var restaurants = await query
            .Skip(filterOptions.PageSize * (filterOptions.PageNumber - 1))
            .Take(filterOptions.PageSize)
            .ToListAsync();

        return (restaurants, totalCount);
    }
}
