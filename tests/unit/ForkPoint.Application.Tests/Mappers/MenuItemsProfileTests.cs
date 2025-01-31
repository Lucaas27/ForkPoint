using AutoMapper;
using FluentAssertions;
using ForkPoint.Application.Mappers;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.CreateMenuItem;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Tests.Mappers;
public class MenuItemsProfileTests
{
    private readonly MapperConfiguration _mapperConfiguration;
    private readonly IMapper _mapper;
    public MenuItemsProfileTests()
    {
        _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<MenuItemsProfile>());
        _mapper = _mapperConfiguration.CreateMapper();
    }

    [Fact]
    public void CreateMap_MapsCorrectly_FromMenuItemToMenuItemModel()
    {
        // Arrange
        var menuItem = new MenuItem
        {
            Id = 1,
            Name = "Burger",
            Description = "A delicious burger",
            Price = 5.99m,
            ImageUrl = "https://www.example.com",
            IsVegan = true,
            IsVegetarian = true,
            KiloCalories = 700,
            RestaurantId = 1,
            Restaurant = new Restaurant()
        };

        // Act
        var menuItemModel = _mapper.Map<MenuItemModel>(menuItem);

        // Assert
        menuItemModel.Should().NotBeNull();
        menuItemModel.Id.Should().Be(menuItem.Id);
        menuItemModel.Name.Should().Be(menuItem.Name);
        menuItemModel.Description.Should().Be(menuItem.Description);
        menuItemModel.Price.Should().Be(menuItem.Price);
        menuItemModel.ImageUrl.Should().Be(menuItem.ImageUrl);
        menuItemModel.IsVegetarian.Should().Be(menuItem.IsVegetarian);
        menuItemModel.IsVegan.Should().Be(menuItem.IsVegan);
        menuItemModel.KiloCalories.Should().Be(menuItem.KiloCalories);
    }


    [Fact]
    public void CreateMap_MapsCorrectly_FromCreateMenuItemRequestToMenuItem()
    {
        // Arrange
        var createMenuItemRequest = new CreateMenuItemRequest
        {
            RestaurantId = 2,
            Name = "Test Name",
            Description = "Test Description",
            Price = 10.00m,
            ImageUrl = "https://test.com",
            IsVegetarian = true,
            IsVegan = true,
            KiloCalories = 700
        };

        // Act
        var menuItem = _mapper.Map<MenuItem>(createMenuItemRequest);

        // Assert
        menuItem.Should().NotBeNull();
        menuItem.RestaurantId.Should().Be(createMenuItemRequest.RestaurantId);
        menuItem.Name.Should().Be(createMenuItemRequest.Name);
        menuItem.Description.Should().Be(createMenuItemRequest.Description);
        menuItem.Price.Should().Be(createMenuItemRequest.Price);
        menuItem.ImageUrl.Should().Be(createMenuItemRequest.ImageUrl);
        menuItem.IsVegetarian.Should().Be(createMenuItemRequest.IsVegetarian);
        menuItem.IsVegan.Should().Be(createMenuItemRequest.IsVegan);
        menuItem.KiloCalories.Should().Be(createMenuItemRequest.KiloCalories);
    }
}
