using AutoMapper;
using FluentAssertions;
using ForkPoint.Application.Contexts;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetCurrentUserRestaurants;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

using ForkPoint.Application.Tests.TestHelpers;


namespace ForkPoint.Application.Tests.Handlers;

public class GetCurrentUserRestaurantsHandlerTests
{
    private readonly Mock<ILogger<GetCurrentUserRestaurantsHandler>> _loggerMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetCurrentUserRestaurantsHandler _handler;

    public GetCurrentUserRestaurantsHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetCurrentUserRestaurantsHandler>>();
        _userManagerMock = TestUserManagerFactory.CreateMinimal();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetCurrentUserRestaurantsHandler(_loggerMock.Object, _userManagerMock.Object, _userContextMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenUserIsNotAuthenticated()
    {
        // Arrange
        _userContextMock.Setup(x => x.GetCurrentUser()).Returns((CurrentUserModel?)null);

        var request = new GetCurrentUserRestaurantsRequest();

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("User not authenticated");
    }

}
