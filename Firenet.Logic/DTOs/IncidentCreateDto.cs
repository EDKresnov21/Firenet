public class IncidentCreateDto
{
    public string Address { get; set; } = null!;
    public string VictimName { get; set; } = null!;
    public int Type { get; set; }
    public string? Description { get; set; }
    public string? Plan { get; set; }
}