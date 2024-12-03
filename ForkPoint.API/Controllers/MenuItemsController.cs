using ForkPoint.Application.Models.Exceptions;
using ForkPoint.Application.Models.Handlers.CreateMenuItem;
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

    [HttpGet("{menuItemId}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<CustomException>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetMenuItemByIdResponse>> GetMenuItemById([FromRoute] int restaurantId, [FromRoute] int menuItemId)
    {

        var response = await mediator.Send(new GetMenuItemByIdRequest(restaurantId, menuItemId));

        return Ok(response);
    }


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

    //// GET api/<MenuItemsController>/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value";
    //}

    //// POST api/<MenuItemsController>
    //public void Post([FromBody] string value)
    //{
    //}

    //// PUT api/<MenuItemsController>/5
    //public void Put(int id, [FromBody] string value)
    //{
    //}

    //// DELETE api/<MenuItemsController>/5
    //public void Delete(int id)
    //{
    //}
}
