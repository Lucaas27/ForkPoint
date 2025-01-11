// Ignore Spelling: Admin

using System.Net.Mime;
using ForkPoint.Application.Models.Exceptions;
using ForkPoint.Application.Models.Handlers.AssignUserRole;
using ForkPoint.Application.Models.Handlers.RemoveUserRole;
using ForkPoint.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForkPoint.API.Controllers;

/// <summary>
///     Controller for admin-related actions.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = AppPolicies.AdminPolicy)]
public class AdminController(IMediator mediator) : ControllerBase
{
    /// <summary>
    ///     Assigns a role to a user.
    /// </summary>
    /// <param name="request">The request containing user and role information.</param>
    /// <returns>The response indicating the result of the operation.</returns>
    [HttpPost("assignUserRole")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<CustomException>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AssignUserRoleResponse>> AssignUserRole([FromBody] AssignUserRoleRequest request)
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }

    /// <summary>
    ///     Removes a role from a user.
    /// </summary>
    /// <param name="request">The request containing user and role information.</param>
    /// <returns>The response indicating the result of the operation.</returns>
    [HttpDelete("removeUserRole")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<CustomException>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RemoveUserRoleResponse>> RemoveUserRole([FromBody] RemoveUserRoleRequest request)
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? NoContent()
            : BadRequest(response);
    }
}