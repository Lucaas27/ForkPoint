﻿using ForkPoint.Application.Models.Handlers.GetAll;
using ForkPoint.Application.Models.Handlers.GetById;
using ForkPoint.Application.Models.Handlers.NewRestaurant;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ForkPoint.API.Controllers;

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
    [ProducesResponseType<GetAllResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var response = await mediator.Send(new GetAllRequest());

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
    [ProducesResponseType<GetByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<GetByIdResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var response = await mediator.Send(new GetByIdRequest(id));

        return response.Restaurant is null ? NotFound(response) : Ok(response);
    }


    /// <summary>
    /// Creates a new restaurant.
    /// </summary>
    /// <param name="command">The details of the new restaurant to create.</param>
    /// <returns>The location of the newly created restaurant.</returns>
    /// <response code="201">Returns the location of the newly created restaurant in the header.</response>
    /// <response code="400">If the restaurant details are invalid.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpPost("new")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<NewRestaurantResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> NewRestaurant([FromBody] NewRestaurantRequest command)
    {
        var response = await mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = response.NewRecordId }, response);
    }

}
