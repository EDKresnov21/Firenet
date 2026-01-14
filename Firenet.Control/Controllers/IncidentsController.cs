using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Firenet.Logic;

[ApiController]
[Route("api/[controller]")]
public class IncidentsController : ControllerBase
{
    private readonly FirenetContext _db;
    private readonly IIncidentService _incidentService;

    public IncidentsController(FirenetContext db, IIncidentService incidentService)
    {
        _db = db;
        _incidentService = incidentService;
    }

    // ---------- SPECIAL ENDPOINTS ----------

    [HttpPost]
    public async Task<ActionResult<Incident>> CreateIncident([FromBody] IncidentCreateDto dto)
    {
        var incident = await _incidentService.CreateIncidentAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = incident.Id }, incident);
    }

    [HttpPost("{id}/close")]
    public async Task<IActionResult> CloseIncident(int id)
    {
        await _incidentService.CloseIncidentAsync(id);
        return NoContent();
    }

    // ---------- CRUD ----------

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Incident>>> GetAll()
        => Ok(await _db.Incidents.ToListAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Incident>> Get(int id)
    {
        var incident = await _db.Incidents.FindAsync(id);
        if (incident == null) return NotFound();
        return Ok(incident);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Incident updated)
    {
        var incident = await _db.Incidents.FindAsync(id);
        if (incident == null) return NotFound();

        incident.Address = updated.Address;
        incident.VictimName = updated.VictimName;
        incident.Type = updated.Type;
        incident.Description = updated.Description;
        incident.Plan = updated.Plan;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var incident = await _db.Incidents.FindAsync(id);
        if (incident == null) return NotFound();

        _db.Incidents.Remove(incident);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
