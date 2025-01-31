using AutoMapper;
using FluentAssertions;
using ForkPoint.Application.Mappers;
using ForkPoint.Application.Models.Handlers.AdminUpdateUserDetails;
using ForkPoint.Application.Models.Handlers.UpdateUser;
using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Tests.Mappers;

public class UserProfileTests
{
    private readonly MapperConfiguration _mapperConfiguration;
    private readonly IMapper _mapper;

    public UserProfileTests()
    {
        _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>());
        _mapper = _mapperConfiguration.CreateMapper();
    }

    [Fact]
    public void CreateMap_MapsCorrectly_FromUpdateUserDetailsRequestToUser()
    {
        // Arrange
        var request = new UpdateUserDetailsRequest("Test Name");

        // Act
        var user = _mapper.Map<User>(request);

        // Assert
        user.Should().NotBeNull();
        user.FullName.Should().Be(request.FullName);
    }

    [Fact]
    public void CreateMap_MapsCorrectly_FromAdminUpdateUserDetailsRequestToUser()
    {
        // Arrange
        var request = new AdminUpdateUserDetailsRequest("Admin Update");

        // Act
        var user = _mapper.Map<User>(request);

        // Assert
        user.Should().NotBeNull();
        user.FullName.Should().Be(request.FullName);

    }
}
