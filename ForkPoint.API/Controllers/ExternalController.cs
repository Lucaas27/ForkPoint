using System.Net.Mime;
using ForkPoint.Application.Models.Handlers.GetExternalRestaurants;
using ForkPoint.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ForkPoint.API.Controllers;

[Route("api/external")]
public class ExternalController(IMediator mediator) : ControllerBase
{
    [HttpGet("restaurants")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetExternalRestaurantsResponse>> GetExternalRestaurants(
        [FromQuery] int pageNumber = 1,
        [FromQuery] PageSizeOptions pageSize = PageSizeOptions.Ten
    )
    {
        var request = new GetExternalRestaurantsRequest(pageNumber, pageSize);
        var response = await mediator.Send(request);

        return Ok(response);
    }
}
