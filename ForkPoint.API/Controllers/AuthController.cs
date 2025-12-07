using System.Net.Mime;
using ForkPoint.Application.Models.Exceptions;
using ForkPoint.Application.Models.Handlers.ExternalProviderCallback;
using ForkPoint.Application.Models.Handlers.LoginUser;
using ForkPoint.Application.Models.Handlers.Logout;
using ForkPoint.Application.Models.Handlers.RefreshToken;
using ForkPoint.Application.Models.Handlers.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace ForkPoint.API.Controllers;

/// <summary>
///     Controller for handling authentication-related actions.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    ///     Registers a new user.
    /// </summary>
    /// <param name="request">The registration request containing user details.</param>
    /// <returns>A response indicating the result of the registration.</returns>
    [HttpPost("register")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RegisterResponse>> RegisterUser([FromBody] RegisterRequest request)
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }

    /// <summary>
    ///     Logs in a user.
    /// </summary>
    /// <param name="request">The login request containing user credentials.</param>
    /// <returns>A response indicating the result of the login attempt.</returns>
    [HttpPost("login")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginResponse>> LoginUser([FromBody] LoginRequest request)
    {
        var response = await mediator.Send(request);

        return response.IsSuccess
            ? Ok(response)
            : Unauthorized(response);
    }

    /// <summary>
    ///     Logs out the current user.
    /// </summary>
    /// <returns>A response indicating the result of the logout attempt.</returns>
    [HttpPost("logout")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LogoutResponse>> Logout()
    {
        var response = await mediator.Send(new LogoutRequest());

        return response.IsSuccess
            ? Ok(response)
            : StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    /// <summary>
    ///     Initiates the Google login process.
    /// </summary>
    /// <returns>A challenge result to initiate Google authentication.</returns>
    [HttpGet("google-login")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public IActionResult GoogleLogin()
    {
        const string authenticationScheme = GoogleDefaults.AuthenticationScheme;
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(ExternalProviderCallback)),
            Items =
            {
                { "LoginProvider", authenticationScheme }
            }
        };
        return Challenge(properties, authenticationScheme);
    }

    /// <summary>
    ///     Handles the callback from an external authentication provider.
    /// </summary>
    /// <returns>A response indicating the result of the external provider callback.</returns>
    [HttpGet("callback")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ExternalProviderResponse>> ExternalProviderCallback()
    {
        var response = await mediator.Send(new ExternalProviderRequest());

        return Ok(response);
    }

    /// <summary>
    ///     Refreshes the authentication token.
    /// </summary>
    /// <returns>A response indicating the result of the token refresh attempt.</returns>
    [HttpPost("refresh-token")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginResponse>> RefreshToken()
    {
        // Refresh token is read from the HttpOnly cookie
        // The access token should be provided in the Authorization header as a Bearer token.
        var authHeader = Request.Headers.Authorization.FirstOrDefault();
        string accessToken = string.Empty;

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            accessToken = authHeader.Substring("Bearer ".Length).Trim();
        }

        var cmd = new RefreshTokenRequest(accessToken);
        var response = await mediator.Send(cmd);

        return response.IsSuccess ? Ok(response) : Unauthorized(response);
    }
}
