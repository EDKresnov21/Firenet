using Microsoft.EntityFrameworkCore;

public class IncidentService : IIncidentService
{
    private readonly FirenetContext _db;
    private readonly Random _random = new();

    public IncidentService(FirenetContext db) => _db = db;

    public async Task<Incident> CreateIncidentAsync(IncidentCreateDto dto)
    {
        var incident = new Incident
        {
            Address = dto.Address,
            VictimName = dto.VictimName,
            Type = dto.Type,
            Description = dto.Description,
            Plan = dto.Plan,
            StartTime = DateTime.UtcNow
        };

        _db.Incidents.Add(incident);
        await _db.SaveChangesAsync();

        await TryAssignCarToIncidentAsync(incident.Id);
        return incident;
    }

    private async Task TryAssignCarToIncidentAsync(int incidentId)
    {
        var incident = await _db.Incidents.FindAsync(incidentId);
        if (incident == null) return;

        var freeCar = await _db.Cars
            .Where(c => c.OnDuty && c.Free)
            .OrderBy(c => c.Id)
            .FirstOrDefaultAsync();

        if (freeCar == null) return;

        var teamIds = await _db.Teams
            .Where(t => t.CarId == freeCar.Id && t.CurrentTeam)
            .Select(t => t.Id)
            .ToListAsync();

        if (!teamIds.Any()) return;

        freeCar.Free = false;

        foreach (var teamId in teamIds)
        {
            _db.IncidentTeams.Add(new IncidentTeam
            {
                IncidentId = incident.Id,
                TeamId = teamId,
                WaterNeeded = GenerateWaterNeeded(incident.Type)
            });
        }

        await _db.SaveChangesAsync();
    }

    private int GenerateWaterNeeded(int type) =>
        type switch
        {
            1 => _random.Next(100, 1001),
            2 => _random.Next(2000, 15001),
            3 => _random.Next(0, 10001),
            _ => _random.Next(100, 1001)
        };

    public async Task CloseIncidentAsync(int incidentId)
    {
        var incident = await _db.Incidents
            .Include(i => i.IncidentTeams)
            .ThenInclude(it => it.Team)
            .ThenInclude(t => t.Car)
            .FirstOrDefaultAsync(i => i.Id == incidentId);

        if (incident == null) return;

        incident.EndTime = DateTime.UtcNow;

        var cars = incident.IncidentTeams
            .Select(it => it.Team.Car)
            .Distinct()
            .ToList();

        foreach (var car in cars)
            car.Free = true;

        await _db.SaveChangesAsync();

        await AssignCarsToWaitingIncidentsAsync();
    }

    private async Task AssignCarsToWaitingIncidentsAsync()
    {
        var waitingIncidents = await _db.Incidents
            .Include(i => i.IncidentTeams)
            .Where(i => i.EndTime == null && !i.IncidentTeams.Any())
            .OrderBy(i => i.StartTime)
            .ToListAsync();

        foreach (var incident in waitingIncidents)
            await TryAssignCarToIncidentAsync(incident.Id);
    }
}
