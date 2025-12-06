using AutoMapper;
using FluentAssertions;
using ForkPoint.Application.Contexts;
using ForkPoint.Application.Mappers;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.CreateRestaurant;
using ForkPoint.Application.Models.Handlers.UpdateRestaurant;
using ForkPoint.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace ForkPoint.Application.Tests.Mappers;

public class RestaurantsProfileTests
{
    private readonly MapperConfiguration _mapperConfiguration;
    private readonly IMapper _mapper;

    public RestaurantsProfileTests()
    {
        // Register a dummy IUserContext so AutoMapper can construct OwnedByCurrentUserResolver
        var services = new ServiceCollection();
        services.AddSingleton<IUserContext>(new DummyUserContext());
        services.AddSingleton(sp => new OwnedByCurrentUserResolver(sp.GetRequiredService<IUserContext>()));
        var provider = services.BuildServiceProvider();

        _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<RestaurantsProfile>());
        _mapper = _mapperConfiguration.CreateMapper(type => provider.GetService(type)!);
    }

    private class DummyUserContext : IUserContext
    {
        public bool IsInRole(string role) => false;
        public CurrentUserModel? GetCurrentUser() => null;
        public int GetTargetUserId() => 0;
    }

    [Fact]
    public void CreateMap_MapsCorrectly_FromRestaurantToRestaurantModel()
    {
        // Arrange
        var restaurant = new Restaurant
        {
            Id = 1,
            OwnerId = 3,
            Name = "Foo",
            Description = "Bar",
            Category = "Cafe",
            HasDelivery = true,
            Email = "test@example.com",
            ContactNumber = "123456789"
        };

        // Act
        var restaurantModel = _mapper.Map<RestaurantModel>(restaurant);

        // Assert
        restaurantModel.Should().NotBeNull();
        restaurantModel.Id.Should().Be(restaurant.Id);
        restaurantModel.Name.Should().Be(restaurant.Name);
        restaurantModel.Description.Should().Be(restaurant.Description);
        restaurantModel.Category.Should().Be(restaurant.Category);
    }

    [Fact]
    public void CreateMap_MapsCorrectly_FromRestaurantToRestaurantDetailsModel()
    {
        // Arrange
        var restaurant = new Restaurant
        {
            Id = 1,
            OwnerId = 3,
            Name = "Foo",
            Description = "Bar",
            Category = "Cafe",
            HasDelivery = true,
            Email = "test@example.com",
            ContactNumber = "123456789",
        };

        // Act
        var restaurantDetailsModel = _mapper.Map<RestaurantDetailsModel>(restaurant);

        // Assert
        restaurantDetailsModel.Should().NotBeNull();
        restaurantDetailsModel.Id.Should().Be(restaurant.Id);
        restaurantDetailsModel.Name.Should().Be(restaurant.Name);
        restaurantDetailsModel.Description.Should().Be(restaurant.Description);
        restaurantDetailsModel.Category.Should().Be(restaurant.Category);
        restaurantDetailsModel.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDetailsModel.Email.Should().Be(restaurant.Email);
        restaurantDetailsModel.ContactNumber.Should().Be(restaurant.ContactNumber);
        restaurantDetailsModel.Address.Should().BeNull();
        restaurantDetailsModel.MenuItems.Should().BeEmpty();
    }


    [Fact]
    public void CreateMap_MapsCorrectly_FromCreateRestaurantRequestToRestaurant()
    {
        // Arrange
        var createRestaurantRequest = new CreateRestaurantRequest
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Cafe",
            HasDelivery = true,
            Email = "test@example.com",
            ContactNumber = "123456789",
        };

        // Act
        var restaurant = _mapper.Map<Restaurant>(createRestaurantRequest);

        // Assert
        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(createRestaurantRequest.Name);
        restaurant.Description.Should().Be(createRestaurantRequest.Description);
        restaurant.HasDelivery.Should().Be(createRestaurantRequest.HasDelivery);
        restaurant.Email.Should().Be(createRestaurantRequest.Email);
        restaurant.ContactNumber.Should().Be(createRestaurantRequest.ContactNumber);
        restaurant.Address.Should().BeNull();
    }

    [Fact]
    public void CreateMap_MapsCorrectly_FromUpdateRestaurantRequestToRestaurant()
    {
        // Arrange
        var updateRestaurantRequest = new UpdateRestaurantRequest
        {
            Name = "Updated Restaurant",
            Description = "Updated Description",
            HasDelivery = false,
            Email = "updated@example.com",
            ContactNumber = "987654321",
            Address = new AddressModel
            {
                Street = "456 Updated St",
                City = "Updated City",
                County = "Updated County",
                PostCode = "54321",
                Country = "Updated Country"
            }
        };

        var existingRestaurant = new Restaurant
        {
            Name = "Old Restaurant",
            Description = "Old Description",
            HasDelivery = true,
            Email = "old@example.com",
            ContactNumber = "123456789",
            Address = new Address
            {
                Street = "123 Old St",
                City = "Old City",
                County = "Old County",
                PostCode = "12345",
                Country = "Old Country"
            }
        };

        // Act
        var restaurant = _mapper.Map(updateRestaurantRequest, existingRestaurant);

        // Assert
        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(updateRestaurantRequest.Name);
        restaurant.Description.Should().Be(updateRestaurantRequest.Description);
        restaurant.HasDelivery.Should().Be(updateRestaurantRequest.HasDelivery.Value);
        restaurant.Email.Should().Be(updateRestaurantRequest.Email);
        restaurant.ContactNumber.Should().Be(updateRestaurantRequest.ContactNumber);
        restaurant.Address.Should().NotBeNull();
        restaurant.Address.Street.Should().Be(updateRestaurantRequest.Address.Street);
        restaurant.Address.City.Should().Be(updateRestaurantRequest.Address.City);
        restaurant.Address.County.Should().Be(updateRestaurantRequest.Address.County);
        restaurant.Address.PostCode.Should().Be(updateRestaurantRequest.Address.PostCode);
        restaurant.Address.Country.Should().Be(updateRestaurantRequest.Address.Country);
    }
}
