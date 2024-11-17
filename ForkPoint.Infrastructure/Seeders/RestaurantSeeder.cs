using ForkPoint.Domain.Entities;
using ForkPoint.Infrastructure.Persistence;

namespace ForkPoint.Infrastructure.Seeders;
internal class RestaurantSeeder(ForkPointDbContext dbContext) : ISeeder
{
    private IEnumerable<Restaurant> RestaurantList
    {
        get
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
                            PostCode = "PE22 7WE",
                            Country = "UK"
                        },
                    MenuItems =
                        [
                            new MenuItem
                            {
                                Name = "coffee",
                                Price = 10.00m,
                                Description = "Coffee with milk",
                                KiloCalories = 100

                            },
                            new MenuItem
                            {
                                Name = "chicken",
                                Price = 20.00m,
                                Description = "Fried chicken",
                                KiloCalories = 500
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
                        PostCode = "PE22 7WE",
                        Country = "UK"
                    },
                    MenuItems =
                        [
                            new MenuItem
                            {
                                Name = "Big Mac",
                                Price = 30.00m,
                                Description = "Big Mac with fries",
                                KiloCalories = 300
                            },
                            new MenuItem
                            {
                                Name = "Ice cream",
                                Price = 9.00m,
                                Description = "Vanilla ice cream",
                                KiloCalories = 200
                            }
                        ],
                    }
            };

            return restaurants;
        }
    }

    public async Task Seed()
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                dbContext.Restaurants.AddRange(RestaurantList);
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
