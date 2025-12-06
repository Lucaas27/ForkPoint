using AutoMapper;
using FluentAssertions;
using ForkPoint.Application.Contexts;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.CreateRestaurant;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForkPoint.Application.Tests.Handlers;

public class CreateRestaurantHandlerTests
{
    private readonly Mock<ILogger<CreateRestaurantHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly CreateRestaurantHandler _handler;

    public CreateRestaurantHandlerTests()
    {
        _loggerMock = new Mock<ILogger<CreateRestaurantHandler>>();
        _mapperMock = new Mock<IMapper>();
        _restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        _userContextMock = new Mock<IUserContext>();
        _handler = new CreateRestaurantHandler(_loggerMock.Object, _mapperMock.Object, _restaurantRepositoryMock.Object, _userContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateRestaurant_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateRestaurantRequest();
        var restaurant = new Restaurant();
        var currentUser = new CurrentUserModel(5, "user@example.com", [], "User");

        _userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _mapperMock.Setup(x => x.Map<Restaurant>(request)).Returns(restaurant);
        _restaurantRepositoryMock.Setup(x => x.CreateRestaurantAsync(It.IsAny<Restaurant>())).ReturnsAsync(1);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.NewRecordId.Should().Be(1);
        restaurant.OwnerId.Should().Be(currentUser.Id);
        _restaurantRepositoryMock.Verify(x => x.CreateRestaurantAsync(It.IsAny<Restaurant>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        var request = new CreateRestaurantRequest();

        _userContextMock.Setup(x => x.GetCurrentUser()).Returns((CurrentUserModel?)null);

        // Act
        var action = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage("User not found");
        _restaurantRepositoryMock.Verify(x => x.CreateRestaurantAsync(It.IsAny<Restaurant>()), Times.Never);

    }
}
