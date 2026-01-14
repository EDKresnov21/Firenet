public class Car
{
    public int Id { get; set; }
    public bool OnDuty { get; set; }
    public bool Free { get; set; }

    public ICollection<Team> Teams { get; set; } = new List<Team>();
}