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
}