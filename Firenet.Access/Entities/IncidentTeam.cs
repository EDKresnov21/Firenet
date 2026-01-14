public class IncidentTeam
{
    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;

    public int IncidentId { get; set; }
    public Incident Incident { get; set; } = null!;

    public int WaterNeeded { get; set; }
}