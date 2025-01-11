using System.Net.Mime;
using ForkPoint.Application.Models.Exceptions;
using ForkPoint.Application.Models.Handlers.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForkPoint.API.Controllers;

/// <summary>
///     Controller for handling account-related actions such as email confirmation, password reset, and user details
///     update.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController(IMediator mediator) : ControllerBase
{
    /// <summary>
    ///     Updates the user's details.
    /// </summary>
    /// <param name="detailsRequest">The request containing the new user details.</param>
    /// <returns>A response indicating the result of the update operation.</returns>
    [HttpPatch("update")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<CustomException>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UpdateUserDetailsResponse>> UpdateUserDetails(
        [FromBody] UpdateUserDetailsRequest detailsRequest
    )
    {
        var response = await mediator.Send(detailsRequest);
        return response.IsSuccess
            ? NoContent()
            : BadRequest(response);
    }
}