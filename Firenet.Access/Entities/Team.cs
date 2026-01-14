public class Team
{
    public int Id { get; set; }

    public int CarId { get; set; }
    public Car Car { get; set; } = null!;

    public int FighterId { get; set; }
    public Firefighter Fighter { get; set; } = null!;

    public bool CurrentTeam { get; set; }

    public ICollection<IncidentTeam> IncidentTeams { get; set; } = new List<IncidentTeam>();
}