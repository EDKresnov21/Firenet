using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Firenet.Logic;

[ApiController]
[Route("api/[controller]")]
public class IncidentTeamsController : ControllerBase
{
    private readonly FirenetContext _db;

    public IncidentTeamsController(FirenetContext db)
    {
        _db = db;
    }

    // GET ALL
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncidentTeam>>> GetAll()
        => Ok(await _db.IncidentTeams
            .Include(it => it.Team)
            .Include(it => it.Incident)
            .ToListAsync());

    // GET BY KEYS
    [HttpGet("{teamId}/{incidentId}")]
    public async Task<ActionResult<IncidentTeam>> Get(int teamId, int incidentId)
    {
        var it = await _db.IncidentTeams
            .Include(x => x.Team)
            .Include(x => x.Incident)
            .FirstOrDefaultAsync(x => x.TeamId == teamId && x.IncidentId == incidentId);

        if (it == null) return NotFound();
        return Ok(it);
    }

    // CREATE
    [HttpPost]
    public async Task<ActionResult<IncidentTeam>> Create(IncidentTeam it)
    {
        _db.IncidentTeams.Add(it);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { teamId = it.TeamId, incidentId = it.IncidentId }, it);
    }

    // UPDATE
    [HttpPut("{teamId}/{incidentId}")]
    public async Task<IActionResult> Update(int teamId, int incidentId, IncidentTeam updated)
    {
        var it = await _db.IncidentTeams
            .FirstOrDefaultAsync(x => x.TeamId == teamId && x.IncidentId == incidentId);

        if (it == null) return NotFound();

        it.WaterNeeded = updated.WaterNeeded;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE
    [HttpDelete("{teamId}/{incidentId}")]
    public async Task<IActionResult> Delete(int teamId, int incidentId)
    {
        var it = await _db.IncidentTeams
            .FirstOrDefaultAsync(x => x.TeamId == teamId && x.IncidentId == incidentId);

        if (it == null) return NotFound();

        _db.IncidentTeams.Remove(it);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
