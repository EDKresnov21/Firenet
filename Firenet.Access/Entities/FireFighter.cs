public class Firefighter
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public bool Absent { get; set; }
    public DateTime? VacationStart { get; set; }
    public DateTime? VacationEnd { get; set; }
    public int Task { get; set; } // 1-4

    public ICollection<Team> Teams { get; set; } = new List<Team>();
}