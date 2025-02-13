using FluentAssertions;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Handlers.DeleteAllMenuItems;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForkPoint.Application.Tests.Handlers;

public class DeleteAllMenuItemsHandlerTests
{
    private readonly Mock<ILogger<DeleteAllMenuItemsHandler>> _loggerMock;
    private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock;
    private readonly Mock<IMenuRepository> _menuRepositoryMock;
    private readonly DeleteAllMenuItemsHandler _handler;

    public DeleteAllMenuItemsHandlerTests()
    {
        _loggerMock = new Mock<ILogger<DeleteAllMenuItemsHandler>>();
        _restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        _menuRepositoryMock = new Mock<IMenuRepository>();
        _handler = new DeleteAllMenuItemsHandler(_loggerMock.Object, _restaurantRepositoryMock.Object, _menuRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_RestaurantExists_DeletesAllMenuItems()
    {
        // Arrange
        var restaurantId = 1;
        var request = new DeleteAllMenuItemsRequest(restaurantId);
        var restaurant = new Restaurant { Id = restaurantId, MenuItems = new List<MenuItem> { new(), new() } };

        _restaurantRepositoryMock.Setup(repo => repo.GetRestaurantByIdAsync(restaurantId)).ReturnsAsync(restaurant);
        _menuRepositoryMock.Setup(repo => repo.DeleteAllMenuItemsAsync(It.IsAny<IEnumerable<MenuItem>>())).Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Be($"All menu items for restaurant {restaurantId} were deleted");
        _restaurantRepositoryMock.Verify(repo => repo.GetRestaurantByIdAsync(restaurantId), Times.Once);
        _menuRepositoryMock.Verify(repo => repo.DeleteAllMenuItemsAsync(restaurant.MenuItems), Times.Once);
    }

    [Fact]
    public async Task Handle_RestaurantDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var restaurantId = 1;
        var request = new DeleteAllMenuItemsRequest(restaurantId);

        _restaurantRepositoryMock.Setup(repo => repo.GetRestaurantByIdAsync(restaurantId)).ReturnsAsync((Restaurant?)null);

        // Act
        var action = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
        _restaurantRepositoryMock.Verify(repo => repo.GetRestaurantByIdAsync(restaurantId), Times.Once);
        _menuRepositoryMock.Verify(repo => repo.DeleteAllMenuItemsAsync(It.IsAny<IEnumerable<MenuItem>>()), Times.Never);
    }
}
