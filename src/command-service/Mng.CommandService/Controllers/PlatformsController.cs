using Microsoft.AspNetCore.Mvc;

namespace Mng.CommandService.Controllers;

[ApiController]
[Route("api/v1/platforms")]
public class PlatformsController : ControllerBase
{
    private ILogger<PlatformsController> _logger;

    public PlatformsController(ILogger<PlatformsController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public ActionResult Post()
    {
        _logger.LogInformation("PlatformsController post");

        return Ok();
    }
}