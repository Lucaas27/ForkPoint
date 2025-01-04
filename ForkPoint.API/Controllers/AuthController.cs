using System.Net.Mime;
using ForkPoint.Application.Models.Handlers.EmailConfirmation;
using ForkPoint.Application.Models.Handlers.ExternalProviderCallback;
using ForkPoint.Application.Models.Handlers.ForgotPassword;
using ForkPoint.Application.Models.Handlers.LoginUser;
using ForkPoint.Application.Models.Handlers.RefreshToken;
using ForkPoint.Application.Models.Handlers.RegisterUser;
using ForkPoint.Application.Models.Handlers.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace ForkPoint.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RegisterResponse>> RegisterUser([FromBody] RegisterRequest request)
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }

    [HttpPost("login")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginResponse>> LoginUser([FromBody] LoginRequest request)
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? Ok(response)
            : Unauthorized(response);
    }


    [HttpGet("confirmEmail")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ConfirmEmailResponse>> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }


    [HttpGet("google")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [HttpGet("callback")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ExternalProviderResponse>> ExternalProviderCallback()
    {
        var response = await mediator.Send(new ExternalProviderRequest());
        return Ok(response);
    }

    [HttpPost("refreshToken")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }

    [HttpPost("forgotPassword")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }

    [HttpPost("resetPassword")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ResetPasswordResponse>> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }
}