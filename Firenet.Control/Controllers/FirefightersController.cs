using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Firenet.Logic;

[ApiController]
[Route("api/[controller]")]
public class FirefightersController : ControllerBase
{
    private readonly FirenetContext _db;
    private readonly ITeamService _teamService;

    public FirefightersController(FirenetContext db, ITeamService teamService)
    {
        _db = db;
        _teamService = teamService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Firefighter>>> GetAll()
        => Ok(await _db.Firefighters.ToListAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Firefighter>> Get(int id)
    {
        var f = await _db.Firefighters.FindAsync(id);
        if (f == null) return NotFound();
        return Ok(f);
    }

    [HttpPost]
    public async Task<ActionResult<Firefighter>> Create(Firefighter firefighter)
    {
        _db.Firefighters.Add(firefighter);
        await _db.SaveChangesAsync();
        await _teamService.RearrangeTeamsOnChangeAsync();
        return CreatedAtAction(nameof(Get), new { id = firefighter.Id }, firefighter);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Firefighter updated)
    {
        var f = await _db.Firefighters.FindAsync(id);
        if (f == null) return NotFound();

        f.Name = updated.Name;
        f.Surname = updated.Surname;
        f.Absent = updated.Absent;
        f.VacationStart = updated.VacationStart;
        f.VacationEnd = updated.VacationEnd;
        f.Task = updated.Task;

        await _db.SaveChangesAsync();
        await _teamService.RearrangeTeamsOnChangeAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var f = await _db.Firefighters.FindAsync(id);
        if (f == null) return NotFound();

        bool isBusy = await _db.Teams
            .Include(t => t.Car)
            .AnyAsync(t => t.FighterId == id && !t.Car.Free);

        _db.Firefighters.Remove(f);
        await _db.SaveChangesAsync();

        if (!isBusy)
            await _teamService.RearrangeTeamsOnChangeAsync(removedFighterId: id);

        return NoContent();
    }
}
