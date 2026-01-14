public class Incident
{
    public int Id { get; set; }
    public string Address { get; set; } = null!;
    public string VictimName { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Type { get; set; } // 1-3
    public string? Description { get; set; }
    public string? Plan { get; set; }

    public ICollection<IncidentTeam> IncidentTeams { get; set; } = new List<IncidentTeam>();
}

