using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mng.CommandService.Data;
using Mng.CommandService.DataContracts;

namespace Mng.CommandService.Controllers;

[ApiController]
[Route("api/v1/platforms")]
public class PlatformsController : ControllerBase
{
    private readonly ILogger<PlatformsController> _logger;
    private readonly IMapper _mapper;
    private readonly CommandContext _context;

    public PlatformsController(
        ILogger<PlatformsController> logger,
        IMapper mapper,
        CommandContext context
    )
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDataContract>>> Get()
    {
        var platforms = await _context.Platforms.ToListAsync();
        var platformDataContracts = _mapper.Map<IEnumerable<PlatformReadDataContract>>(platforms);

        return Ok(platformDataContracts);
    }

    [HttpPost]
    public ActionResult Post()
    {
        _logger.LogInformation("PlatformsController post");

        return Ok();
    }
}