using FluentAssertions;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Handlers.DeleteMenuItem;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForkPoint.Application.Tests.Handlers;

public class DeleteMenuItemHandlerTests
{
    private readonly Mock<ILogger<DeleteMenuItemHandler>> _loggerMock;
    private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock;
    private readonly Mock<IMenuRepository> _menuRepositoryMock;
    private readonly DeleteMenuItemHandler _handler;

    public DeleteMenuItemHandlerTests()
    {
        _loggerMock = new Mock<ILogger<DeleteMenuItemHandler>>();
        _restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        _menuRepositoryMock = new Mock<IMenuRepository>();
        _handler = new DeleteMenuItemHandler(_loggerMock.Object, _restaurantRepositoryMock.Object, _menuRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteMenuItem_WhenMenuItemExists()
    {
        // Arrange
        var restaurantId = 1;
        var menuItemId = 1;
        var request = new DeleteMenuItemRequest(restaurantId, menuItemId);
        var restaurant = new Restaurant
        {
            Id = restaurantId,
            MenuItems = new List<MenuItem> { new() { Id = menuItemId } }
        };

        _restaurantRepositoryMock.Setup(repo => repo.GetRestaurantByIdAsync(restaurant.Id)).ReturnsAsync(restaurant);
        _menuRepositoryMock.Setup(repo => repo.DeleteMenuItemAsync(It.IsAny<MenuItem>())).Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Be($"Menu item id {menuItemId} deleted successfully.");
        _menuRepositoryMock.Verify(repo => repo.DeleteMenuItemAsync(It.Is<MenuItem>(item => item.Id == menuItemId)), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenRestaurantDoesNotExist()
    {
        // Arrange
        var restaurantId = 1;
        var menuItemId = 1;
        var request = new DeleteMenuItemRequest(restaurantId, menuItemId);

        _restaurantRepositoryMock.Setup(repo => repo.GetRestaurantByIdAsync(restaurantId)).ReturnsAsync((Restaurant?)null);

        // Act
        var action = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenMenuItemDoesNotExist()
    {
        // Arrange
        var restaurantId = 1;
        var menuItemId = 1;
        var request = new DeleteMenuItemRequest(restaurantId, menuItemId);
        var restaurant = new Restaurant { Id = restaurantId, MenuItems = new List<MenuItem>() };

        _restaurantRepositoryMock.Setup(repo => repo.GetRestaurantByIdAsync(restaurantId)).ReturnsAsync(restaurant);

        // Act
        var action = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
    }
}
