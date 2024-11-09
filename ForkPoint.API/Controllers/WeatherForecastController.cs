using Microsoft.AspNetCore.Mvc;

namespace ForkPoint.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{


    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IWeatherForecastService weatherForecastService

        )
    {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }

    /// <summary>
    /// Gets weather forecast.
    /// </summary>
    /// <remarks>
    /// This method retrieves a list of weather forecasts from the weather forecast service.
    /// The forecasts include the date, temperature in Celsius, and a summary.
    /// </remarks>
    /// <returns>List of forecasts</returns>
    /// <response code="200">Returns the forecasts</response>
    /// <response code="400">Something is wrong in the request</response>
    [HttpGet]
    //[Produces("application/json")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IEnumerable<WeatherForecast> Get()
    {
        return _weatherForecastService.Get();
    }

}
