using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Firenet.Logic;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _stats;

    public StatisticsController(IStatisticsService stats)
    {
        _stats = stats;
    }

    [HttpGet("firefighters-incidents-last-year")]
    public async Task<ActionResult<IEnumerable<FirefighterIncidentCountDto>>> GetFirefighterIncidentCounts()
        => Ok(await _stats.GetFirefighterIncidentCountsLastYearAsync());

    [HttpGet("incident-times-last-year")]
    public async Task<ActionResult<IncidentTimeStatsDto>> GetIncidentTimeStats()
        => Ok(await _stats.GetIncidentTimeStatsLastYearAsync());

    [HttpGet("car-efficiency")]
    public async Task<ActionResult<IEnumerable<CarEfficiencyDto>>> GetCarEfficiency()
        => Ok(await _stats.GetCarEfficiencyByYearAsync());
}