using Firenet.Interface.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Firenet.Interface.Services;

namespace Firenet.Interface.Pages;

public class IndexModel : PageModel
{
    private readonly StatisticsServiceUI _stats;

    public List<int> OnDutyCars { get; set; } = new();
    public List<FirefighterIncidentCountDto> FirefighterStats { get; set; } = new();
    public IncidentTimeStatsDto IncidentStats { get; set; } = new();

    public IndexModel(StatisticsServiceUI stats)
    {
        _stats = stats;
    }

    public async Task OnGet()
    {
        OnDutyCars = (await _stats.GetOnDutyCarIdsAsync()).ToList();
        FirefighterStats = (await _stats.GetFirefighterIncidentCountsLastYearAsync()).ToList();
        IncidentStats = await _stats.GetIncidentTimeStatsLastYearAsync();
    }
}
