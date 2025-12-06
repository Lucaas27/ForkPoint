using AutoMapper;
using FluentAssertions;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetRestaurantById;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForkPoint.Application.Tests.Handlers;

public class GetRestaurantByIdHandlerTests
{
    private readonly Mock<ILogger<GetRestaurantByIdHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantRepository> _repoMock;
    private readonly GetRestaurantByIdHandler _handler;

    public GetRestaurantByIdHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetRestaurantByIdHandler>>();
        _mapperMock = new Mock<IMapper>();
        _repoMock = new Mock<IRestaurantRepository>();
        _handler = new GetRestaurantByIdHandler(_loggerMock.Object, _mapperMock.Object, _repoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnRestaurant_WhenFound()
    {
        // Arrange
        var restaurant = new Restaurant { Id = 7, Name = "R" , Description = "D", Category = "C", HasDelivery = true, Email = "a@b.com", ContactNumber = null, Address = new Address(), MenuItems = new List<MenuItem>(), Owner = new User() };
        var dto = new RestaurantDetailsModel { Id = 7, Name = "R", Category = "C", HasDelivery = true, Address = new AddressModel() };

        _repoMock.Setup(x => x.GetRestaurantByIdAsync(7)).ReturnsAsync(restaurant);
        _mapperMock.Setup(m => m.Map<RestaurantDetailsModel>(restaurant)).Returns(dto);

        // Act
        var result = await _handler.Handle(new GetRestaurantByIdRequest(7), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Restaurant.Should().NotBeNull();
        result.Restaurant.Id.Should().Be(7);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenRestaurantMissing()
    {
        // Arrange
        _repoMock.Setup(x => x.GetRestaurantByIdAsync(99)).ReturnsAsync((Restaurant?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(new GetRestaurantByIdRequest(99), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}

