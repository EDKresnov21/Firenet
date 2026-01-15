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
        var query =
            from f in _db.Firefighters
            join t in _db.Teams on f.Id equals t.FighterId
            join it in _db.IncidentTeams on t.Id equals it.TeamId
            join i in _db.Incidents on it.IncidentId equals i.Id
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

        var incidents = await _db.Incidents
            .Where(i => i.EndTime != null)
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
        // 1) Load ALL required rows into memory
        var data = await (
            from it in _db.IncidentTeams
            join t in _db.Teams on it.TeamId equals t.Id
            join c in _db.Cars on t.CarId equals c.Id
            join i in _db.Incidents on it.IncidentId equals i.Id
            select new
            {
                CarId = c.Id,
                Year = i.StartTime.Year,
                Water = it.WaterNeeded,
                IncidentId = i.Id
            }
        ).ToListAsync(); // <-- THIS FIXES THE EF ERROR

        // 2) Now do grouping in memory (EF cannot translate it)
        var result = data
            .GroupBy(x => new { x.CarId, x.Year })
            .Select(g =>
            {
                var incidentCount = g.Select(x => x.IncidentId).Distinct().Count();
                var totalWater = g.Sum(x => x.Water);

                return new CarEfficiencyDto
                {
                    CarId = g.Key.CarId,
                    Year = g.Key.Year,
                    TotalWater = totalWater,
                    IncidentCount = incidentCount,
                    Efficiency = incidentCount == 0 ? 0 : (double)totalWater / incidentCount
                };
            })
            .OrderBy(r => r.Year)
            .ThenByDescending(r => r.Efficiency)
            .ToList();

        return result;
    }


}
