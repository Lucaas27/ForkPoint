using ForkPoint.Application.Restaurants;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ForkPoint.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantsController(IRestaurantsService restaurantsService) : ControllerBase
{
    private readonly IRestaurantsService _restaurantsService = restaurantsService;

    // GET api/Restaurants
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<IEnumerable<Restaurant>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync()
    {
        var restaurants = await _restaurantsService.GetAllAsync();

        return Ok(restaurants);
    }

    // GET api/Restaurants/<id>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<Restaurant>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var restaurant = await _restaurantsService.GetByIdAsync(id);

        return restaurant is null ? NotFound() : Ok(restaurant);
    }
}
