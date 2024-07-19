using Microsoft.AspNetCore.Mvc;

namespace CodingExerciseApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class WeatherForecastController: ControllerBase
{
    [HttpGet]
    public IActionResult GetThings()
    {
        return Ok("Thing");

    }
}