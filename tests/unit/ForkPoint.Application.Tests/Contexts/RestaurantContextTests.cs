using FluentAssertions;
using ForkPoint.Application.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace ForkPoint.Application.Tests.Contexts;

// TestMethod__ExpectedResult__Scenario
public class RestaurantContextTests
{

    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
    private readonly Mock<HttpContext> _httpContextMock = new();
    private readonly Mock<HttpRequest> _requestMock = new();

    [Fact]
    public void GetTargetedRestaurantId_ReturnsId_WhenIdIsValid()
    {
        // Arrange
        var routeValues = new RouteValueDictionary { { "restaurantId", "123" } };

        _requestMock.SetupGet(r => r.RouteValues).Returns(routeValues);
        _httpContextMock.SetupGet(c => c.Request).Returns(_requestMock.Object);
        _httpContextAccessorMock.SetupGet(h => h.HttpContext).Returns(_httpContextMock.Object);

        var context = new RestaurantContext(_httpContextAccessorMock.Object);

        // Act
        var result = context.GetTargetedRestaurantId();

        // Assert
        result.Should().Be(123);
        result.Should().BeOfType(typeof(int));
    }

    [Fact]
    public void GetTargetedRestaurantId_ThrowsInvalidOperationException_WhenIdIsMissing()
    {
        // Arrange
        var routeValues = new RouteValueDictionary();

        _requestMock.Setup(r => r.RouteValues).Returns(routeValues);
        _httpContextMock.Setup(c => c.Request).Returns(_requestMock.Object);
        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(_httpContextMock.Object);

        var context = new RestaurantContext(_httpContextAccessorMock.Object);

        // Act
        Action act = () => context.GetTargetedRestaurantId();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetTargetedRestaurantId_ThrowsInvalidCastException_WhenIdIsNotAnInt()
    {
        // Arrange
        var routeValues = new RouteValueDictionary { { "restaurantId", "abc" } };

        _requestMock.Setup(r => r.RouteValues).Returns(routeValues);
        _httpContextMock.Setup(c => c.Request).Returns(_requestMock.Object);
        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(_httpContextMock.Object);

        var context = new RestaurantContext(_httpContextAccessorMock.Object);

        // Act
        Action act = () => context.GetTargetedRestaurantId();

        //Assert
        act.Should().Throw<InvalidCastException>();
    }
}
