using ForkPoint.Application.Restaurants;
using ForkPoint.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ForkPoint.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantsController : ControllerBase
{
    // GET api/Restaurants
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<IEnumerable<Restaurant>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(IRestaurantsService restaurantsService)
    {
        var restaurants = await restaurantsService.GetRestaurantsAsync();

        return Ok(restaurants);
    }

}
