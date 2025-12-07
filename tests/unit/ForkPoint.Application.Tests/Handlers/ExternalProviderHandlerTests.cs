using FluentAssertions;
using ForkPoint.Application.Handlers;
using ForkPoint.Application.Models.Handlers.ExternalProviderCallback;
using ForkPoint.Application.Services;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace ForkPoint.Application.Tests.Handlers;

public class ExternalProviderHandlerTests
{
    private readonly Mock<ILogger<ExternalProviderHandler>> _loggerMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly ExternalProviderHandler _handler;
    private static readonly User _user = new() { UserName = "testuser", Email = "test@example.com" };
    private const string _dummyRefreshToken = "mock_refresh_token";
    private const string _dummyToken = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ0ZXN0In0.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

    public ExternalProviderHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ExternalProviderHandler>>();
        _authServiceMock = MockAuthService();
        _userRepositoryMock = MockUserRepository();
        _signInManagerMock = MockSignInManager();

        _handler = new ExternalProviderHandler(
            _loggerMock.Object,
            _authServiceMock.Object,
            _userRepositoryMock.Object,
            _signInManagerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResponse_WhenExternalProviderLoginSucceeds()
    {
        // Arrange
        var request = new ExternalProviderRequest();

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.AccessToken.Should().Be(_dummyToken);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenExternalLoginInfoIsNull()
    {
        // Arrange
        var request = new ExternalProviderRequest();
        _signInManagerMock.Setup(s => s.GetExternalLoginInfoAsync(It.IsAny<string>()))
            .ReturnsAsync((ExternalLoginInfo?)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();

    }

    [Fact]
    public async Task Handle_ShouldCreateInternalUserAndAssociateExternalLogin_WhenExternalUserIsNull()
    {
        // Arrange
        var request = new ExternalProviderRequest();
        _userRepositoryMock.Setup(u => u.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((User?)null);
        _userRepositoryMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.AccessToken.Should().Be(_dummyToken);
        _userRepositoryMock.Verify(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        _userRepositoryMock.Verify(u => u.AddLoginAsync(It.IsAny<User>(), It.IsAny<UserLoginInfo>()), Times.Once);
    }

    private static Mock<UserManager<User>> MockUserManager()
    {
        var store = new Mock<IUserStore<User>>();
        var options = Options.Create(new IdentityOptions());

        // Setup PasswordHasher
        var passwordHasher = new Mock<IPasswordHasher<User>>();
        passwordHasher.Setup(p => p.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
            .Returns("hashed_password");

        // Setup UserValidator
        var userValidatorMock = new Mock<IUserValidator<User>>();
        userValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<UserManager<User>>(), It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);
        var userValidators = new List<IUserValidator<User>> { userValidatorMock.Object };

        // Setup PasswordValidator
        var passwordValidatorMock = new Mock<IPasswordValidator<User>>();
        passwordValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<UserManager<User>>(), It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        var passwordValidators = new List<IPasswordValidator<User>> { passwordValidatorMock.Object };

        // Setup KeyNormalizer
        var keyNormalizer = new Mock<ILookupNormalizer>();
        keyNormalizer.Setup(n => n.NormalizeName(It.IsAny<string>())).Returns((string name) => name);
        keyNormalizer.Setup(n => n.NormalizeEmail(It.IsAny<string>())).Returns((string email) => email);

        // Use a concrete instance for IdentityErrorDescriber
        var errors = new IdentityErrorDescriber();

        // Setup Services
        var services = new Mock<IServiceProvider>();
        services.Setup(s => s.GetService(typeof(ILogger<UserManager<User>>)))
            .Returns(new Mock<ILogger<UserManager<User>>>().Object);

        var logger = new Mock<ILogger<UserManager<User>>>();

        var mockUserManager = new Mock<UserManager<User>>(
            store.Object,
            options,
            passwordHasher.Object,
            userValidators,
            passwordValidators,
            keyNormalizer.Object,
            errors,
            services.Object,
            logger.Object
        );

        // In-memory user store for simulating user creation and retrieval
        var users = new List<User>();

        // Setup CreateAsync to add the user to the in-memory list
        mockUserManager.Setup(u => u.CreateAsync(It.IsAny<User>()))
            .Callback<User>((user) => users.Add(user))
            .ReturnsAsync(IdentityResult.Success);

        mockUserManager.Setup(s => s.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string name) => users.FirstOrDefault(u => u.UserName == name));

        mockUserManager.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((string email) => users.FirstOrDefault(u => u.Email == email));

        mockUserManager.Setup(u => u.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        mockUserManager.Setup(u => u.AddLoginAsync(It.IsAny<User>(), It.IsAny<UserLoginInfo>()))
            .ReturnsAsync(IdentityResult.Success);

        // Setup AddToRoleAsync to return success
        mockUserManager.Setup(u => u.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Setup GetRolesAsync to return an empty list
        mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string>());

        return mockUserManager;
    }

    private static Mock<IUserRepository> MockUserRepository()
    {
        var mock = new Mock<IUserRepository>();
        var users = new List<User>();

        mock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Callback<User, string?>((user, pw) => users.Add(user))
            .ReturnsAsync(IdentityResult.Success);

        mock.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((string email) => users.FirstOrDefault(u => u.Email == email));

        mock.Setup(u => u.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        mock.Setup(u => u.AddLoginAsync(It.IsAny<User>(), It.IsAny<UserLoginInfo>()))
            .ReturnsAsync(IdentityResult.Success);

        mock.Setup(u => u.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        mock.Setup(u => u.GetRolesAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string>());

        return mock;
    }


    private static Mock<SignInManager<User>> MockSignInManager()
    {
        var userManager = MockUserManager();
        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
        var mockSignInManager = new Mock<SignInManager<User>>
            (
                userManager.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null!,
                null!,
                null!,
                null!
            );

        mockSignInManager.Setup(s => s.GetExternalLoginInfoAsync(It.IsAny<string>()))
            .ReturnsAsync(
            new ExternalLoginInfo(
                new ClaimsPrincipal(), "provider", "providerKey", "displayName"
                )
            );

        return mockSignInManager;
    }


    private static Mock<IAuthService> MockAuthService()
    {
        var mockAuthService = new Mock<IAuthService>();
        // Mock GenerateAccessToken to return any string (token value doesn't matter since we won't parse it)
        mockAuthService.Setup(a => a.GenerateAccessToken(It.IsAny<User>()))
            .ReturnsAsync(_dummyToken);

        mockAuthService.Setup(a => a.GenerateRefreshToken(It.IsAny<User>()))
            .ReturnsAsync(_dummyRefreshToken);

        // If GetPrincipalFromToken is called, return a mock ClaimsPrincipal
        mockAuthService.Setup(a => a.GetPrincipalFromToken(It.IsAny<string>()))
            .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, _user.UserName!) })));

        mockAuthService.Setup(a => a.ValidateRefreshToken(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        mockAuthService.Setup(a => a.InvalidateRefreshToken(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        return mockAuthService;
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserCreationFails()
    {
        // Arrange
        var request = new ExternalProviderRequest();
        _userRepositoryMock.Setup(u => u.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((User?)null);
        _userRepositoryMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);
        _userRepositoryMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed" }));

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Failed to create user: User creation failed");
    }

}
