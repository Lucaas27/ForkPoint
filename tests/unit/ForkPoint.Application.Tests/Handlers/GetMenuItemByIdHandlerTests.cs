using AutoMapper;
using FluentAssertions;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetMenuItemById;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForkPoint.Application.Tests.Handlers;

public class GetMenuItemByIdHandlerTests
{
    private readonly Mock<ILogger<GetMenuItemByIdHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantRepository> _repoMock;
    private readonly GetMenuItemByIdHandler _handler;

    public GetMenuItemByIdHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetMenuItemByIdHandler>>();
        _mapperMock = new Mock<IMapper>();
        _repoMock = new Mock<IRestaurantRepository>();
        _handler = new GetMenuItemByIdHandler(_loggerMock.Object, _mapperMock.Object, _repoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnMenuItem_WhenFound()
    {
        // Arrange
        var menuItem = new MenuItem { Id = 5, Name = "M", Description = "D", Price = 10.0m, RestaurantId = 1 };
        var restaurant = new Restaurant { Id = 1, MenuItems = new List<MenuItem> { menuItem }, Name = "R", Description = "D", Category = "C", HasDelivery = false, Email = "e@e.com", ContactNumber = null, Address = new Address(), Owner = new User() };
        var dto = new MenuItemModel { Id = 5, Name = "M", Description = "D", Price = 10.0m };

        _repoMock.Setup(r => r.GetRestaurantByIdAsync(1)).ReturnsAsync(restaurant);
        _mapperMock.Setup(m => m.Map<MenuItemModel>(It.IsAny<MenuItem>())).Returns(dto);

        // Act
        var result = await _handler.Handle(new GetMenuItemByIdRequest(1, 5), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.MenuItem.Should().NotBeNull();
        result.MenuItem.Id.Should().Be(5);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenRestaurantMissing()
    {
        // Arrange
        _repoMock.Setup(r => r.GetRestaurantByIdAsync(99)).ReturnsAsync((Restaurant?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(new GetMenuItemByIdRequest(99, 1), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenMenuItemMissing()
    {
        // Arrange
        var restaurant = new Restaurant { Id = 2, MenuItems = new List<MenuItem>(), Name = "R", Description = "D", Category = "C", HasDelivery = false, Email = "e@e.com", ContactNumber = null, Address = new Address(), Owner = new User() };
        _repoMock.Setup(r => r.GetRestaurantByIdAsync(2)).ReturnsAsync(restaurant);

        // Act
        Func<Task> act = async () => await _handler.Handle(new GetMenuItemByIdRequest(2, 999), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}

