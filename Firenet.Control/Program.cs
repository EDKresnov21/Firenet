using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FirenetContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FirenetDb")));

builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FirenetContext>();
    db.Database.Migrate();
    SeedIfEmpty(db);

    var teamService = scope.ServiceProvider.GetRequiredService<ITeamService>();
    await teamService.ResetAndArrangeCurrentTeamsAsync();
}

app.MapControllers();
app.Run();

static void SeedIfEmpty(FirenetContext db)
{
    if (!db.Cars.Any())
    {
        db.Cars.AddRange(
            new Car { OnDuty = true, Free = true },
            new Car { OnDuty = true, Free = true }
        );
        db.Firefighters.AddRange(
            new Firefighter { Name = "John", Surname = "Doe" },
            new Firefighter { Name = "Jane", Surname = "Smith" },
            new Firefighter { Name = "Alex", Surname = "Brown" }
        );
        db.SaveChanges();
    }
}