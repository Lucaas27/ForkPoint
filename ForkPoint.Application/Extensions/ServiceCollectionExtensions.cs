using ForkPoint.Application.Restaurants;
using Microsoft.Extensions.DependencyInjection;

namespace ForkPoint.Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantsService, RestaurantsService>();
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
    }
}
