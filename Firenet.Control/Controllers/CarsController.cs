using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Firenet.Logic;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly FirenetContext _db;
    private readonly IStatisticsService _stats;

    public CarsController(FirenetContext db, IStatisticsService stats)
    {
        _db = db;
        _stats = stats;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        => Ok(await _db.Cars.ToListAsync());

    [HttpPost]
    public async Task<ActionResult<Car>> CreateCar(Car car)
    {
        _db.Cars.Add(car);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Car>> GetCar(int id)
    {
        var car = await _db.Cars.FindAsync(id);
        if (car == null) return NotFound();
        return Ok(car);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(int id, [FromServices] ITeamService teamService)
    {
        var car = await _db.Cars.FindAsync(id);
        if (car == null) return NotFound();

        bool isBusy = !car.Free;
        _db.Cars.Remove(car);
        await _db.SaveChangesAsync();

        if (!isBusy)
            await teamService.RearrangeTeamsOnChangeAsync(removedCarId: id);

        return NoContent();
    }

    [HttpGet("on-duty-ids")]
    public async Task<ActionResult<IEnumerable<int>>> GetOnDutyCarIds()
    {
        var ids = await _stats.GetOnDutyCarIdsAsync();
        return Ok(ids);
    }
}