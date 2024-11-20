using ForkPoint.Application.Services.Restaurant;
using ForkPoint.Application.Services.Restaurants;
using Microsoft.Extensions.DependencyInjection;

namespace ForkPoint.Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantService, RestaurantService>();
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
    }
}
