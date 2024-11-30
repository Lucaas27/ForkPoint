using ForkPoint.Application.Models.Exceptions;
using ForkPoint.Application.Models.Handlers.CreateRestaurant;
using ForkPoint.Application.Models.Handlers.DeleteRestaurant;
using ForkPoint.Application.Models.Handlers.GetAll;
using ForkPoint.Application.Models.Handlers.GetById;
using ForkPoint.Application.Models.Handlers.UpdateRestaurant;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ForkPoint.API.Controllers;

/// <summary>
/// Controller for managing restaurant-related operations.
/// </summary>
/// <param name="mediator">The mediator instance for handling requests.</param>
[Route("api/[controller]")]
[ApiController]
public class RestaurantsController(IMediator mediator) : ControllerBase
{

    // GET api/Restaurants
    /// <summary>
    /// Retrieves all restaurants.
    /// </summary>
    /// <returns>A list of restaurants.</returns>
    /// <response code="200">Returns the list of restaurants.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetAllRestaurantsResponse>> GetAllRestaurants()
    {
        var response = await mediator.Send(new GetAllRestaurantsRequest());

        return Ok(response);
    }


    // GET api/Restaurants/<id>
    /// <summary>
    /// Retrieves a restaurant by its ID.
    /// </summary>
    /// <param name="id">The ID of the restaurant.</param>
    /// <returns>The restaurant with the specified ID.</returns>
    /// <response code="200">Returns the restaurant with the specified ID.</response>
    /// <response code="404">If the restaurant is not found.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CustomException>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetRestaurantByIdResponse>> GetRestaurantById([FromRoute] int id)
    {
        var response = await mediator.Send(new GetRestaurantByIdRequest(id));

        return Ok(response);
    }

    /// <summary>
    /// Creates a new restaurant.
    /// </summary>
    /// <param name="command">The details of the new restaurant to create.</param>
    /// <returns>The location of the newly created restaurant.</returns>
    /// <response code="201">Returns the location of the newly created restaurant in the header.</response>
    /// <response code="400">If the restaurant details are invalid.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpPost("create")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<CustomException>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreateRestaurantResponse>> CreateRestaurant([FromBody] CreateRestaurantRequest command)
    {
        var response = await mediator.Send(command);

        return CreatedAtAction(nameof(GetRestaurantById), new { id = response.NewRecordId }, response);
    }


    /// <summary>
    /// Deletes a restaurant by its ID.
    /// </summary>
    /// <param name="id">The ID of the restaurant to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <response code="204">Returns no content if the deletion is successful.</response>
    /// <response code="404">If the restaurant is not found.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpDelete("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<CustomException>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DeleteRestaurantResponse>> DeleteRestaurant([FromRoute] int id)
    {
        await mediator.Send(new DeleteRestaurantRequest(id));

        return NoContent();
    }


    /// <summary>
    /// Updates a restaurant by its ID.
    /// </summary>
    /// <param name="id">The ID of the restaurant to update.</param>
    /// <param name="command">The details of the restaurant to update.</param>
    /// <returns>No content if the update is successful.</returns>
    /// <response code="204">Returns no content if the update is successful.</response>
    /// <response code="404">If the restaurant is not found.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpPatch("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<CustomException>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UpdateRestaurantResponse>> UpdateRestaurant([FromRoute] int id, [FromBody] UpdateRestaurantRequest command)
    {
        command = command with { Id = id };
        await mediator.Send(command);

        return NoContent();
    }

}
