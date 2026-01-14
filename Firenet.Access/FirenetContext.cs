using Microsoft.EntityFrameworkCore;

public class FirenetContext : DbContext
{
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<Firefighter> Firefighters => Set<Firefighter>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<IncidentTeam> IncidentTeams => Set<IncidentTeam>();

    public FirenetContext(DbContextOptions<FirenetContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IncidentTeam>()
            .HasKey(it => new { it.TeamId, it.IncidentId });

        modelBuilder.Entity<IncidentTeam>()
            .HasOne(it => it.Team)
            .WithMany(t => t.IncidentTeams)
            .HasForeignKey(it => it.TeamId);

        modelBuilder.Entity<IncidentTeam>()
            .HasOne(it => it.Incident)
            .WithMany(i => i.IncidentTeams)
            .HasForeignKey(it => it.IncidentId);
    }
}