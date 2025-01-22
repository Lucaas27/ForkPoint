using System.Security.Claims;
using FluentAssertions;
using ForkPoint.Application.Contexts;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace ForkPoint.Application.Tests.Contexts;

// TestMethod__ExpectedResult__Scenario
public class UserContextTests
{
    private readonly Mock<IHttpContextAccessor> _mockHttpAccessor = new();

    [Theory]
    [InlineData(AppUserRoles.User)]
    [InlineData(AppUserRoles.Admin)]
    [InlineData(AppUserRoles.Owner)]
    public void IsInRole_ReturnsTrue_WhenUserIsInRole(string roleName)
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "currentUser@test.com"),
            new(ClaimTypes.Role, roleName)
        };
        var identity = new ClaimsIdentity(claims, "TestIdentity");
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _mockHttpAccessor.SetupGet(x => x.HttpContext).Returns(httpContext);

        var userContext = new UserContext(_mockHttpAccessor.Object);

        // Act
        var result = userContext.IsInRole(roleName);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppUserRoles.Admin)]
    [InlineData(AppUserRoles.Owner)]
    public void IsInRole_ReturnsFalse_WhenUserIsNotInRole(string roleName)
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "currentUser@test.com"),
            new(ClaimTypes.Role, AppUserRoles.User)
        };
        var identity = new ClaimsIdentity(claims, "TestIdentity");
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _mockHttpAccessor.SetupGet(x => x.HttpContext).Returns(httpContext);

        var userContext = new UserContext(_mockHttpAccessor.Object);

        // Act
        var result = userContext.IsInRole(roleName);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(AppUserRoles.User)]
    [InlineData(AppUserRoles.Admin)]
    [InlineData(AppUserRoles.Owner)]
    public void IsInRole_ReturnsFalse_WhenRoleIsNotFound(string roleName)
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "currentUser@test.com"),
            new(ClaimTypes.Role, roleName)
        };
        var identity = new ClaimsIdentity(claims, "TestIdentity");
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _mockHttpAccessor.SetupGet(x => x.HttpContext).Returns(httpContext);

        var userContext = new UserContext(_mockHttpAccessor.Object);

        // Act
        var result = userContext.IsInRole(roleName.ToLower());

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("1", "currentUser@test.com", AppUserRoles.User)]
    [InlineData("2", "currentUser2@test.com", AppUserRoles.Admin)]
    [InlineData("3", "currentUser3@test.com", AppUserRoles.Owner)]
    public void GetCurrentUser_ReturnsCurrentUser_WhenUserIsAuthenticated(string id, string email, string role)
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, id),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role)
        };
        var identity = new ClaimsIdentity(claims, "TestIdentity");
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _mockHttpAccessor.SetupGet(x => x.HttpContext).Returns(httpContext);

        var userContext = new UserContext(_mockHttpAccessor.Object);

        // Act
        var result = userContext.GetCurrentUser();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(int.Parse(id));
        result.Email.Should().Be(email);
        result.Roles.Should().Contain(role);
        result.Should().BeOfType(typeof(CurrentUserModel));
    }

    [Fact]
    public void GetCurrentUser_ReturnsNull_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var identity = new ClaimsIdentity();
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _mockHttpAccessor.SetupGet(x => x.HttpContext).Returns(httpContext);

        var userContext = new UserContext(_mockHttpAccessor.Object);

        // Act
        var result = userContext.GetCurrentUser();

        // Assert
        result.Should().BeNull();
    }


    [Fact]
    public void GetTargetUserId_ReturnsUserId_WhenUserIdIsValid()
    {
        // Arrange
        var routeValues = new RouteValueDictionary { { "userId", "1" } };
        var httpContext = new DefaultHttpContext { Request = { RouteValues = routeValues } };
        _mockHttpAccessor.SetupGet(x => x.HttpContext).Returns(httpContext);

        var userContext = new UserContext(_mockHttpAccessor.Object);

        // Act
        var result = userContext.GetTargetUserId();

        // Assert
        result.Should().Be(1);
        result.Should().BeOfType(typeof(int));
    }

    [Fact]
    public void GetTargetUserId_ThrowsInvalidOperationException_WhenUserIdIsNotFound()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _mockHttpAccessor.SetupGet(x => x.HttpContext).Returns(httpContext);

        var userContext = new UserContext(_mockHttpAccessor.Object);

        // Act
        Action action = () => userContext.GetTargetUserId();

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("User ID not found in the current context");
    }

    [Fact]
    public void GetTargetUserId_ThrowsInvalidCastException_WhenUserIdIsNotAnInt()
    {
        // Arrange
        var routeValues = new RouteValueDictionary { { "userId", "abc" } };
        var httpContext = new DefaultHttpContext { Request = { RouteValues = routeValues } };
        _mockHttpAccessor.SetupGet(x => x.HttpContext).Returns(httpContext);

        var userContext = new UserContext(_mockHttpAccessor.Object);

        // Act
        Action action = () => userContext.GetTargetUserId();


        // Assert
        action.Should().Throw<InvalidCastException>()
            .WithMessage("User ID is not an INT");
    }
}