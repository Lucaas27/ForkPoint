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
using System.Net.Mime;

namespace ForkPoint.API.Controllers;

/// <summary>
/// Controller for handling authentication-related actions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Registers a new user.
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
    /// Logs in a user.
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
    /// Logs out the current user.
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
    /// Initiates the Google login process.
    /// </summary>
    /// <returns>A challenge result to initiate Google authentication.</returns>
    [HttpGet("googleLogin")]
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
    /// Handles the callback from an external authentication provider.
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
    /// Refreshes the authentication token.
    /// </summary>
    /// <param name="request">The refresh token request containing the current token.</param>
    /// <returns>A response indicating the result of the token refresh attempt.</returns>
    [HttpPost("refreshToken")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }
}
