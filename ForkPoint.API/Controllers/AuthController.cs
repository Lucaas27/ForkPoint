using System.Net.Mime;
using ForkPoint.Application.Models.Handlers.ExternalProviderCallback;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace ForkPoint.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpGet("google")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GoogleLogin()
    {
        var authenticationScheme = GoogleDefaults.AuthenticationScheme;
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(ExternalProviderCallback), "Auth",
                new { authenticationScheme }),
            Items =
            {
                { "LoginProvider", authenticationScheme },
                { "ReturnUrl", "/" }
            }
        };
        return Challenge(properties, authenticationScheme);
    }

    [HttpGet("callback")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ExternalProviderResponse>> ExternalProviderCallback(
        [FromQuery] string authenticationScheme)
    {
        var requestWithCtx = new ExternalProviderRequest(HttpContext, authenticationScheme);
        var response = await mediator.Send(requestWithCtx);
        return Ok(response);
    }
}