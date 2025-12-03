using Microsoft.AspNetCore.Mvc;

namespace ForkPoint.API.Controllers;

/// <summary>
///     Health check controller for monitoring application status.
/// </summary>
[Route("health")]
[ApiController]
public class HealthController : ControllerBase
{
    /// <summary>
    ///     Basic health check endpoint.
    /// </summary>
    /// <returns>Health status of the application.</returns>
    [HttpGet]
    [Produces("application/json")]
    public IActionResult GetHealth()
    {
        return Ok("Healthy");
    }

    /// <summary>
    ///     Detailed health check endpoint.
    /// </summary>
    /// <returns>Detailed health information.</returns>
    [HttpGet("info")]
    [Produces("application/json")]
    public IActionResult GetInfoHealth()
    {
        var healthInfo = new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
        };

        return Ok(healthInfo);
    }
}