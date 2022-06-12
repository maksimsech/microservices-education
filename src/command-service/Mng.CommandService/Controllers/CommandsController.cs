using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mng.CommandService.Data;
using Mng.CommandService.Data.Models;
using Mng.CommandService.DataContracts;

namespace Mng.CommandService.Controllers;

[ApiController]
[Route("api/v1/platforms/{platformsId}/commands")]
public class CommandsController : ControllerBase
{
    private readonly CommandContext _context;
    private readonly IMapper _mapper;

    public CommandsController(CommandContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommandReadDataContract>>> Get(Guid platformId)
    {
        var isPlatformExists = await IsPlatformExists(platformId);
        if (!isPlatformExists)
        {
            return NotFound();
        }

        var commands = _context.Commands
            .Where(c => c.PlatformId == platformId)
            .ToListAsync();
        var commandsDataContracts = _mapper.Map<IEnumerable<CommandReadDataContract>>(commands);

        return Ok(commandsDataContracts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommandReadDataContract>> GetById(Guid platformId, Guid id)
    {
        var isPlatformExists = await IsPlatformExists(platformId);
        if (!isPlatformExists)
        {
            return NotFound();
        }

        var command = await _context.Commands.FirstOrDefaultAsync(c => c.PlatformId == platformId && c.Id == id);
        if (command is null)
        {
            return NotFound();
        }

        var commandDataContract = _mapper.Map<CommandReadDataContract>(command);

        return Ok(commandDataContract);
    }

    [HttpPost]
    public async Task<ActionResult<CommandReadDataContract>> Post(
        Guid platformId,
        CommandCreateDataContract commandCreate
    )
    {
        var isPlatformExists = await IsPlatformExists(platformId);
        if (!isPlatformExists)
        {
            return BadRequest();
        }

        var command = _mapper.Map<Command>(commandCreate);

        _context.Add(command);
        await _context.SaveChangesAsync();

        var commandDataContract = _mapper.Map<CommandReadDataContract>(command);

        return CreatedAtAction(
            nameof(GetById), 
            new { platformId, id = commandDataContract.Id },
            commandDataContract
        );
    }
    
    private async Task<bool> IsPlatformExists(Guid platformId) => await _context.Platforms.AnyAsync(p => p.Id == platformId);
}