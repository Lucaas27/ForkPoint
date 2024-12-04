using ForkPoint.Application.Models.Exceptions;
using ForkPoint.Application.Models.Handlers.CreateMenuItem;
using ForkPoint.Application.Models.Handlers.DeleteAllMenuItems;
using ForkPoint.Application.Models.Handlers.DeleteMenuItem;
using ForkPoint.Application.Models.Handlers.GetMenuItemById;
using ForkPoint.Application.Models.Handlers.GetMenuItems;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;


namespace ForkPoint.API.Controllers;
[Route("api/restaurant/{restaurantId}/[controller]")]
[ApiController]
public class MenuItemsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retrieves the menu items for a specific restaurant.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <returns>A list of menu items for the specified restaurant.</returns>
    /// <response code="200">Returns the list of menu items.</response>
    /// <response code="404">If the restaurant is not found.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetMenuItemsResponse>> GetMenuItems([FromRoute] int restaurantId)
    {
        var response = await mediator.Send(new GetMenuItemsRequest(restaurantId));

        return Ok(response);
    }

    /// <summary>
    /// Retrieves a specific menu item by its ID for a specific restaurant.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <param name="menuItemId">The ID of the menu item.</param>
    /// <returns>The menu item for the specified restaurant and menu item ID.</returns>
    /// <response code="204">If the menu item is found.</response>
    /// <response code="404">If the menu item or restaurant is not found.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpGet("{menuItemId}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<CustomException>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetMenuItemByIdResponse>> GetMenuItemById([FromRoute] int restaurantId, [FromRoute] int menuItemId)
    {
        var response = await mediator.Send(new GetMenuItemByIdRequest(restaurantId, menuItemId));

        return Ok(response);
    }

    /// <summary>
    /// Creates a new menu item for a specific restaurant.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <param name="command">The details of the menu item to create.</param>
    /// <returns>The newly created menu item.</returns>
    /// <response code="200">If the menu item is successfully created.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpPost("create")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreateMenuItemResponse>> CreateMenuItem([FromRoute] int restaurantId, [FromBody] CreateMenuItemRequest command)
    {
        var request = command with { RestaurantId = restaurantId };

        var response = await mediator.Send(request);

        return CreatedAtAction(nameof(GetMenuItemById), new { restaurantId, menuItemId = response.NewRecordId }, null);
    }

    /// <summary>
    /// Deletes a specific menu item by its ID for a specific restaurant.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <param name="menuItemId">The ID of the menu item.</param>
    /// <returns>No content.</returns>
    /// <response code="204">If the menu item is successfully deleted.</response>
    /// <response code="404">If the menu item or restaurant is not found.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpDelete("{menuItemId}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<CustomException>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DeleteMenuItemResponse>> DeleteMenuItem([FromRoute] int restaurantId, [FromRoute] int menuItemId)
    {
        await mediator.Send(new DeleteMenuItemRequest(restaurantId, menuItemId));

        return NoContent();
    }

    /// <summary>
    /// Deletes all menu items for a specific restaurant.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <returns>No content.</returns>
    /// <response code="204">If all menu items are successfully deleted.</response>
    /// <response code="404">If the restaurant is not found.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpDelete]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<CustomException>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DeleteAllMenuItemsResponse>> DeleteAllMenuItems([FromRoute] int restaurantId)
    {
        await mediator.Send(new DeleteAllMenuItemsRequest(restaurantId));

        return NoContent();
    }
}

