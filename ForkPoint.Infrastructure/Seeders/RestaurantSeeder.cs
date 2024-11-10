using ForkPoint.Domain.Entities;
using ForkPoint.Infrastructure.Persistence;

namespace ForkPoint.Infrastructure.Seeders;
internal class RestaurantSeeder(ForkPointDbContext dbContext) : ISeeder
{
    private IEnumerable<Restaurant> GetRestaurantList()
    {
        var restaurants = new List<Restaurant>
            {
                new()
                {
                    Name = "KFC",
                    Description = "KFC is an American fast food restaurant chain that specializes in fried chicken.",
                    Category = "Fast Food",
                    HasDelivery = true,
                    Email = "kfc@gmail.com",
                    ContactNumber = "1234567890",
                    Address = new Address
                        {
                            Street = "Street 1",
                            City = "City 1",
                            County = "State 1",
                            PostCode = "PE22 7WE"
                        },
                    MenuItems =
                        [
                            new MenuItem
                            {
                                Name = "coffee",
                                Price = 10.00m
                            },
                            new MenuItem
                            {
                                Name = "chicken",
                                Price = 20.00m
                            }
                        ],
                },
                new()
                {
                    Name = "McDonald's",
                    Description = "McDonald's, is an American multinational fast food chain, founded in 1940 as a restaurant operated by Richard and Maurice McDonald, in San Bernardino, California, United States.",
                    Category = "Fast Food",
                    HasDelivery = false,
                    Email = "mcds@gmail.com",
                    ContactNumber = "0987654321",
                    Address = new Address
                    {
                        Street = "Street 2",
                        City = "City 2",
                        County = "State 2",
                        PostCode = "PE22 7WE"
                    },
                    MenuItems =
                        [
                            new MenuItem
                            {
                                Name = "Item 3",
                                Price = 30.00m
                            },
                            new MenuItem
                            {
                                Name = "Item 4",
                                Price = 40.00m
                            }
                        ],
                    }
            };

        return restaurants;
    }

    public async Task Seed()
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                dbContext.Restaurants.AddRange(GetRestaurantList());
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
