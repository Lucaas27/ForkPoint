using FluentAssertions;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Handlers.DeleteRestaurant;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForkPoint.Application.Tests.Handlers;

public class DeleteRestaurantHandlerTests
{
    private readonly Mock<ILogger<DeleteRestaurantHandler>> _loggerMock;
    private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock;
    private readonly DeleteRestaurantHandler _handler;

    public DeleteRestaurantHandlerTests()
    {
        _loggerMock = new Mock<ILogger<DeleteRestaurantHandler>>();
        _restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        _handler = new DeleteRestaurantHandler(_loggerMock.Object, _restaurantRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_RestaurantExists_DeletesRestaurant()
    {
        // Arrange
        var request = new DeleteRestaurantRequest(1);
        var restaurant = new Restaurant { Id = 1 };
        _restaurantRepositoryMock.Setup(repo => repo.GetRestaurantByIdAsync(request.Id))
            .ReturnsAsync(restaurant);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Message.Should().Be($"Restaurant with id {request.Id} deleted.");
        _restaurantRepositoryMock.Verify(repo => repo.DeleteRestaurant(restaurant), Times.Once);
    }

    [Fact]
    public async Task Handle_RestaurantDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var request = new DeleteRestaurantRequest(1);
        _restaurantRepositoryMock.Setup(repo => repo.GetRestaurantByIdAsync(request.Id))
            .ReturnsAsync((Restaurant?)null);

        // Act
        var action = async () => await _handler.Handle(request, CancellationToken.None);


        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
        _restaurantRepositoryMock.Verify(repo => repo.DeleteRestaurant(It.IsAny<Restaurant>()), Times.Never);
    }
}
