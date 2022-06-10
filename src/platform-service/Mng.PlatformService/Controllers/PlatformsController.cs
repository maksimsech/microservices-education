using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mng.PlatformService.Data;
using Mng.PlatformService.Data.Models;
using Mng.PlatformService.DataContracts;

namespace Mng.PlatformService.Controllers;

[ApiController]
[Route("api/v1/platforms")]
public class PlatformController : ControllerBase
{
    private readonly PlatformContext _context;
    private readonly IMapper _mapper;

    public PlatformController(PlatformContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDataContract>>> Get()
    {
        var platforms = await _context.Platforms.ToListAsync();
        var platformDataContracts = _mapper.Map<IEnumerable<PlatformReadDataContract>>(platforms);

        return Ok(platformDataContracts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<PlatformReadDataContract>>> GetById(Guid id)
    {
        var platform = await _context.Platforms.FirstOrDefaultAsync(p => p.Id == id);
        if (platform is null)
        {
            return NotFound();
        }

        var platformDataContract = _mapper.Map<PlatformReadDataContract>(platform);
        return Ok(platformDataContract);
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDataContract>> Post(PlatformCreateDataContract platformCreate)
    {
        var platform = _mapper.Map<Platform>(platformCreate);

        _context.Add(platform);
        await _context.SaveChangesAsync();

        var platformDataContract = _mapper.Map<PlatformReadDataContract>(platform);

        return CreatedAtAction(nameof(GetById), new { id = platformDataContract.Id }, platform);
    }
}