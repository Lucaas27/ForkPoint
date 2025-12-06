using System.Net.Mime;
using ForkPoint.Application.Models.Exceptions;
using ForkPoint.Application.Models.Handlers;
using ForkPoint.Application.Models.Handlers.AdminUpdateUserDetails;
using ForkPoint.Application.Models.Handlers.EmailConfirmation;
using ForkPoint.Application.Models.Handlers.ForgotPassword;
using ForkPoint.Application.Models.Handlers.GetCurrentUser;
using ForkPoint.Application.Models.Handlers.GetCurrentUserRestaurants;
using ForkPoint.Application.Models.Handlers.ResendEmailConfirmation;
using ForkPoint.Application.Models.Handlers.ResetPassword;
using ForkPoint.Application.Models.Handlers.UpdateUser;
using ForkPoint.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForkPoint.API.Controllers;

/// <summary>
///     Controller for handling account-related actions such as email confirmation, password reset, and user details
///     update.
/// </summary>
[Route("api/account")]
[ApiController]
public class AccountController(IMediator mediator) : ControllerBase
{
    private async Task<ActionResult<TResponse>> HandleUpdateRequest<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<TResponse>
        where TResponse : BaseResponse
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? NoContent()
            : BadRequest(response);
    }

    /// <summary>
    ///     Updates the current user's details.
    /// </summary>
    /// <param name="request">The request containing the new user details.</param>
    /// <returns>A response indicating the result of the update operation.</returns>
    [HttpPatch("update")]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<CustomException>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UpdateUserDetailsResponse>> UpdateUserDetails(
        [FromBody] UpdateUserDetailsRequest request
    )
    {
        return await HandleUpdateRequest<UpdateUserDetailsRequest, UpdateUserDetailsResponse>(request);
    }

    /// <summary>
    ///     Allow Admins to update user details.
    /// </summary>
    /// <param name="request">The request containing the new user details.</param>
    /// <param name="userId">The user id to update</param>
    /// <returns>A response indicating the result of the update operation.</returns>
    [HttpPatch("update/{userId:int}")]
    [Authorize(Policy = AppPolicies.AdminPolicy)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<CustomException>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdminUpdateUserDetailsResponse>> AdminUpdateUserDetails(
        [FromBody] AdminUpdateUserDetailsRequest request,
        [FromRoute] int userId
    )
    {
        return await HandleUpdateRequest<AdminUpdateUserDetailsRequest, AdminUpdateUserDetailsResponse>(request);
    }

    /// <summary>
    ///     Initiates the forgot password process.
    /// </summary>
    /// <param name="request">The forgot password request.</param>
    /// <returns>A response indicating the result of the forgot password process.</returns>
    [HttpPost("forgot-password")]
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
    ///     Resets the user's password.
    /// </summary>
    /// <param name="request">The reset password request.</param>
    /// <returns>A response indicating the result of the password reset.</returns>
    [HttpPost("reset-password")]
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
    ///     Confirms the user's email address.
    /// </summary>
    /// <param name="request">The email confirmation request.</param>
    /// <returns>A response indicating the result of the email confirmation.</returns>
    [HttpGet("verify")]
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
    ///     Resends email with token used to verify user account.
    /// </summary>
    /// <param name="request">The email confirmation request.</param>
    /// <returns>A response indicating the result of the email confirmation.</returns>
    [HttpPost("resend-email-confirmation")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ResendEmailConfirmationResponse>> ResendEmailConfirmation(
        [FromBody] ResendEmailConfirmationRequest request
    )
    {
        var response = await mediator.Send(request);
        return response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }

    /// <summary>
    ///     Retrieves restaurants associated with the current authenticated user.
    /// </summary>
    /// <returns>A response containing the user's restaurants or an error.</returns>
    [HttpGet("restaurants")]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetCurrentUserRestaurantsResponse>> GetCurrentUserRestaurants()
    {
        var response = await mediator.Send(new GetCurrentUserRestaurantsRequest());
        return response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }

    /// <summary>
    ///     Retrieves information about the current authenticated user.
    /// </summary>
    /// <returns>The current user's details or an unauthorized response.</returns>
    [HttpGet("me")]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<CustomException>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetCurrentUserResponse>> GetCurrentUser()
    {
        var response = await mediator.Send(new GetCurrentUserRequest());
        return response.IsSuccess
            ? Ok(response)
            : Unauthorized(response);
    }

}
