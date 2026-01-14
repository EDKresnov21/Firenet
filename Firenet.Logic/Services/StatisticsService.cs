using Microsoft.EntityFrameworkCore;

public class StatisticsService : IStatisticsService
{
    private readonly FirenetContext _db;

    public StatisticsService(FirenetContext db) => _db = db;

    public async Task<IEnumerable<int>> GetOnDutyCarIdsAsync()
    {
        return await _db.Cars
            .Where(c => c.OnDuty)
            .Select(c => c.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<FirefighterIncidentCountDto>> GetFirefighterIncidentCountsLastYearAsync()
    {
        var oneYearAgo = DateTime.UtcNow.AddYears(-1);

        var query =
            from f in _db.Firefighters
            join t in _db.Teams on f.Id equals t.FighterId
            join it in _db.IncidentTeams on t.Id equals it.TeamId
            join i in _db.Incidents on it.IncidentId equals i.Id
            where i.StartTime >= oneYearAgo
            group i by new { f.Id, f.Name, f.Surname } into g
            orderby g.Count() descending
            select new FirefighterIncidentCountDto
            {
                FirefighterId = g.Key.Id,
                Name = g.Key.Name,
                Surname = g.Key.Surname,
                IncidentCount = g.Count()
            };

        return await query.ToListAsync();
    }

    public async Task<IncidentTimeStatsDto> GetIncidentTimeStatsLastYearAsync()
    {
        var oneYearAgo = DateTime.UtcNow.AddYears(-1);

        var incidents = await _db.Incidents
            .Where(i => i.StartTime >= oneYearAgo && i.EndTime != null)
            .ToListAsync();

        if (!incidents.Any())
            return new IncidentTimeStatsDto();

        var durations = incidents
            .Select(i => (i.EndTime!.Value - i.StartTime).TotalHours)
            .ToList();

        return new IncidentTimeStatsDto
        {
            AverageHours = durations.Average(),
            MinHours = durations.Min(),
            MaxHours = durations.Max(),
            IncidentCount = incidents.Count,
            TotalHours = durations.Sum()
        };
    }

    public async Task<IEnumerable<CarEfficiencyDto>> GetCarEfficiencyByYearAsync()
    {
        var query =
            from it in _db.IncidentTeams
            join t in _db.Teams on it.TeamId equals t.Id
            join c in _db.Cars on t.CarId equals c.Id
            join i in _db.Incidents on it.IncidentId equals i.Id
            group new { it, i } by new { c.Id, Year = i.StartTime.Year } into g
            let incidentCount = g.Select(x => x.i.Id).Distinct().Count()
            orderby g.Key.Year, (double)g.Sum(x => x.it.WaterNeeded) / incidentCount descending
            select new CarEfficiencyDto
            {
                CarId = g.Key.Id,
                Year = g.Key.Year,
                TotalWater = g.Sum(x => x.it.WaterNeeded),
                IncidentCount = incidentCount,
                Efficiency = incidentCount == 0 ? 0 : (double)g.Sum(x => x.it.WaterNeeded) / incidentCount
            };

        return await query.ToListAsync();
    }
}
