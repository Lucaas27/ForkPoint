using AutoMapper;
using FluentAssertions;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetAllRestaurants;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Enums;
using ForkPoint.Domain.Models;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForkPoint.Application.Tests.Handlers;

public class GetAllRestaurantsHandlerTests
{
    private readonly Mock<ILogger<GetAllRestaurantsHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock;
    private readonly GetAllRestaurantsHandler _handler;

    public GetAllRestaurantsHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetAllRestaurantsHandler>>();
        _mapperMock = new Mock<IMapper>();
        _restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        _handler = new GetAllRestaurantsHandler(_loggerMock.Object, _mapperMock.Object, _restaurantRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllRestaurants_WhenRequestIsValid()
    {
        // Arrange
        var request = new GetAllRestaurantsRequest(SearchOptions.Name,
            "Test",
            1,
            PageSizeOptions.Ten,
            SortByOptions.Name,
            SortDirection.Ascending)
        ;

        var restaurants = new List<Restaurant>
        {
            new() { Id = 1, Name = "Test Restaurant 1" },
            new() { Id = 2, Name = "Test Restaurant 2" }
        };

        _restaurantRepositoryMock
            .Setup(repo => repo.GetFilteredRestaurantsAsync(It.IsAny<RestaurantFilterOptions>()))
            .ReturnsAsync((restaurants, restaurants.Count));

        _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<RestaurantModel>>(It.IsAny<IEnumerable<Restaurant>>()))
            .Returns(restaurants.Select(r => new RestaurantModel { Id = r.Id, Name = r.Name }));

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Items.Should().HaveCount(2);
        response.Items.First().Name.Should().Be("Test Restaurant 1");
        response.Items.Last().Name.Should().Be("Test Restaurant 2");
    }
}
