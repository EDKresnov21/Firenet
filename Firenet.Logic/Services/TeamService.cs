using Microsoft.EntityFrameworkCore;

public class TeamService : ITeamService
{
    private readonly FirenetContext _db;

    public TeamService(FirenetContext db) => _db = db;

    public async Task ResetAndArrangeCurrentTeamsAsync()
    {
        var currentTeams = _db.Teams.Where(t => t.CurrentTeam);
        _db.Teams.RemoveRange(currentTeams);

        var cars = await _db.Cars.Where(c => c.OnDuty).ToListAsync();
        var fighters = await _db.Firefighters
            .Where(f => !f.Absent)
            .ToListAsync();

        fighters = fighters
            .Where(f => !IsOnVacation(f))
            .ToList();


        ArrangeTeams(cars, fighters);
        await _db.SaveChangesAsync();
    }

    private static bool IsOnVacation(Firefighter f)
    {
        if (!f.VacationStart.HasValue || !f.VacationEnd.HasValue) return false;
        var today = DateTime.UtcNow.Date;
        return today >= f.VacationStart.Value.Date && today <= f.VacationEnd.Value.Date;
    }

    private void ArrangeTeams(List<Car> cars, List<Firefighter> fighters)
    {
        int carsCount = cars.Count;
        int fightersCount = fighters.Count;

        if (carsCount == 0 || fightersCount < 3) return;

        int maxCarsWithTeams = Math.Min(carsCount, fightersCount / 3);

        var selectedCars = cars.Take(maxCarsWithTeams).ToList();
        var fighterQueue = new Queue<Firefighter>(fighters);

        foreach (var car in selectedCars)
        {
            for (int i = 0; i < 3; i++)
            {
                if (fighterQueue.Count == 0) break;
                var fighter = fighterQueue.Dequeue();
                _db.Teams.Add(new Team
                {
                    CarId = car.Id,
                    FighterId = fighter.Id,
                    CurrentTeam = true
                });
            }
        }
    }

    public async Task RearrangeTeamsOnChangeAsync(int? removedCarId = null, int? removedFighterId = null)
    {
        // For now: full rebuild of current teams
        await ResetAndArrangeCurrentTeamsAsync();
    }
}