using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Firenet.Logic;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly FirenetContext _db;

    public TeamsController(FirenetContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Team>>> GetAll()
        => Ok(await _db.Teams
            .Include(t => t.Car)
            .Include(t => t.Fighter)
            .ToListAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Team>> Get(int id)
    {
        var team = await _db.Teams
            .Include(t => t.Car)
            .Include(t => t.Fighter)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (team == null) return NotFound();
        return Ok(team);
    }

    [HttpPost]
    public async Task<ActionResult<Team>> Create(Team team)
    {
        _db.Teams.Add(team);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = team.Id }, team);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Team updated)
    {
        var team = await _db.Teams.FindAsync(id);
        if (team == null) return NotFound();

        team.CarId = updated.CarId;
        team.FighterId = updated.FighterId;
        team.CurrentTeam = updated.CurrentTeam;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var team = await _db.Teams.FindAsync(id);
        if (team == null) return NotFound();

        _db.Teams.Remove(team);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}