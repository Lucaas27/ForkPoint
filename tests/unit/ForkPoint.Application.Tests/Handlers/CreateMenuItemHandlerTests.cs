using AutoMapper;
using FluentAssertions;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Handlers.CreateMenuItem;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForkPoint.Application.Tests.Handlers;

public class CreateMenuItemHandlerTests
{
    private readonly Mock<ILogger<CreateMenuItemHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock;
    private readonly Mock<IMenuRepository> _menuRepositoryMock;
    private readonly CreateMenuItemHandler _handler;

    public CreateMenuItemHandlerTests()
    {
        _loggerMock = new Mock<ILogger<CreateMenuItemHandler>>();
        _mapperMock = new Mock<IMapper>();
        _restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        _menuRepositoryMock = new Mock<IMenuRepository>();
        _handler = new CreateMenuItemHandler(
            _loggerMock.Object,
            _mapperMock.Object,
            _restaurantRepositoryMock.Object,
            _menuRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_RestaurantExists_CreatesMenuItem()
    {
        // Arrange
        var request = new CreateMenuItemRequest();
        var menuItem = new MenuItem();

        _restaurantRepositoryMock.Setup(repo => repo
            .GetRestaurantByIdAsync(request.RestaurantId))
            .ReturnsAsync(new Restaurant());
        _mapperMock.Setup(mapper => mapper.Map<MenuItem>(request)).Returns(menuItem);
        _menuRepositoryMock.Setup(repo => repo.CreateMenuItemAsync(menuItem)).ReturnsAsync(1);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.NewRecordId.Should().Be(1);
        _restaurantRepositoryMock.Verify(repo => repo.GetRestaurantByIdAsync(request.RestaurantId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<MenuItem>(request), Times.Once);
        _menuRepositoryMock.Verify(repo => repo.CreateMenuItemAsync(menuItem), Times.Once);
    }

    [Fact]
    public async Task Handle_RestaurantDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var request = new CreateMenuItemRequest();

        _restaurantRepositoryMock
            .Setup(repo => repo
            .GetRestaurantByIdAsync(request.RestaurantId))
            .ReturnsAsync((Restaurant?)null);

        // Act
        var action = async () => await _handler.Handle(request, CancellationToken.None);


        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
        _restaurantRepositoryMock.Verify(repo => repo.GetRestaurantByIdAsync(request.RestaurantId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<MenuItem>(It.IsAny<CreateMenuItemRequest>()), Times.Never);
        _menuRepositoryMock.Verify(repo => repo.CreateMenuItemAsync(It.IsAny<MenuItem>()), Times.Never);
    }
}
