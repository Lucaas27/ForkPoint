using FluentValidation;
using FluentValidation.AspNetCore;
using ForkPoint.Application.Services.Restaurants;
using Microsoft.Extensions.DependencyInjection;

namespace ForkPoint.Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddScoped<IRestaurantService, RestaurantService>();

        services.AddAutoMapper(applicationAssembly);

        services.AddValidatorsFromAssembly(applicationAssembly)
            .AddFluentValidationAutoValidation();
    }
}
