using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using ForkPoint.Application.Models.Handlers.ExternalProviderCallback;

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
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(ExternalProviderCallback)),
            Items =
            {
                { "LoginProvider", GoogleDefaults.AuthenticationScheme }
            }
        };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("callback")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ExternalProviderResponse>> ExternalProviderCallback()
    {
        var requestWithCtx = new ExternalProviderRequest(HttpContext);
        var response = await mediator.Send(requestWithCtx);
        return response;
    }
}
