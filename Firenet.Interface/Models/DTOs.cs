namespace Firenet.Interface.Models
{
    public class Car
    {
        public int Id { get; set; }
        public bool OnDuty { get; set; }
        public bool Free { get; set; }
    }
    
    public class Firefighter
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public bool Absent { get; set; }
        public DateTime? VacationStart { get; set; }
        public DateTime? VacationEnd { get; set; }
        public int Task { get; set; }
    }
    
    public class Incident
    {
        public int Id { get; set; }
        public string Address { get; set; } = "";
        public string VictimName { get; set; } = "";
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Type { get; set; }
        public string? Description { get; set; }
        public string? Plan { get; set; }
    }
    
    public class IncidentCreateDto
    {
        public string Address { get; set; } = "";
        public string VictimName { get; set; } = "";
        public int Type { get; set; }
        public string? Description { get; set; }
        public string? Plan { get; set; }
    }
    
    public class FirefighterIncidentCountDto
    {
        public int FirefighterId { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public int IncidentCount { get; set; }
    }
    
    public class IncidentTimeStatsDto
    {
        public float AverageHours { get; set; }
        public float MinHours { get; set; }
        public float MaxHours { get; set; }
        public int IncidentCount { get; set; }
        public float TotalHours { get; set; }
    }

    public class CarEfficiencyDto
    {
        public int CarId { get; set; }
        public int Year { get; set; }
        public int TotalWater { get; set; }
        public int IncidentCount { get; set; }
        public double Efficiency { get; set; }
    }

    public class Team
    {
        public int Id { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; } = null!;

        public int FighterId { get; set; }
        public Firefighter Fighter { get; set; } = null!;

        public bool CurrentTeam { get; set; }
    }
}