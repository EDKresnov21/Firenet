using Firenet.Interface.Models;
using Firenet.Interface.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Firenet.Interface.Pages;

public class TeamsModel : PageModel
{
    private readonly TeamsService _service;

    public List<Team> Teams { get; set; } = new();

    public TeamsModel(TeamsService service)
    {
        _service = service;
    }

    public async Task OnGet()
    {
        Teams = await _service.GetTeams();
    }
}