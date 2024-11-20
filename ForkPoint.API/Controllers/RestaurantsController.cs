using ForkPoint.Application.Models.Restaurant;
using ForkPoint.Application.Services.Restaurants;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ForkPoint.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantsController(IRestaurantService restaurantsService) : ControllerBase
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
    [ProducesResponseType<IEnumerable<RestaurantModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await restaurantsService.GetAllAsync();

        return Ok(restaurants);
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
    [ProducesResponseType<RestaurantDetailsModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var restaurant = await restaurantsService.GetByIdAsync(id);

        return restaurant is null ? NotFound() : Ok(restaurant);
    }


    [HttpPost("new")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<NewRestaurantModel>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> NewRestaurant([FromBody] NewRestaurantModel newRestaurant)
    {
        var id = await restaurantsService.CreateAsync(newRestaurant);

        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

}
