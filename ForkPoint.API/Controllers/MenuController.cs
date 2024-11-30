using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetMenu;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace ForkPoint.API.Controllers;
[Route("api/restaurant/{restaurantId}/[controller]")]
[ApiController]
public class MenuController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItemModel>>> GetMenu([FromRoute] int restaurantId)
    {
        var menu = await mediator.Send(new GetMenuRequest(restaurantId));

        return Ok(menu);
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
