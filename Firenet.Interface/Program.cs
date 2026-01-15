using Firenet.Interface.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("FirenetApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5132/");
});

builder.Services.AddScoped<CarsService>();
builder.Services.AddScoped<FirefightersService>();
builder.Services.AddScoped<IncidentsService>();
builder.Services.AddScoped<TeamsService>();
builder.Services.AddScoped<StatisticsServiceUI>();

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

Console.WriteLine("INTERFACE WORKS");