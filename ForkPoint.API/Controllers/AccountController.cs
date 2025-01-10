using ForkPoint.Application.Models.Exceptions;
using ForkPoint.Application.Models.Handlers.EmailConfirmation;
using ForkPoint.Application.Models.Handlers.ForgotPassword;
using ForkPoint.Application.Models.Handlers.ResetPassword;
using ForkPoint.Application.Models.Handlers.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ForkPoint.API.Controllers;

/// <summary>
/// Controller for handling account-related actions such as email confirmation, password reset, and user details update.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AccountController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Confirms the user's email address.
    /// </summary>
    /// <param name="request">The email confirmation request.</param>
    /// <returns>A response indicating the result of the email confirmation.</returns>
    [HttpGet("confirmEmail")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ConfirmEmailResponse>> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }

    /// <summary>
    /// Initiates the forgot password process.
    /// </summary>
    /// <param name="request">The forgot password request.</param>
    /// <returns>A response indicating the result of the forgot password process.</returns>
    [HttpPost("forgotPassword")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }

    /// <summary>
    /// Resets the user's password.
    /// </summary>
    /// <param name="request">The reset password request.</param>
    /// <returns>A response indicating the result of the password reset.</returns>
    [HttpPost("resetPassword")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ResetPasswordResponse>> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }

    /// <summary>
    /// Updates the user's details.
    /// </summary>
    /// <param name="detailsRequest">The request containing the new user details.</param>
    /// <returns>A response indicating the result of the update operation.</returns>
    [HttpPatch("updateUserDetails")]
    [Authorize]
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
