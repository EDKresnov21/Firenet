using Firenet.Interface.Models;
using Firenet.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Firenet.Interface.Pages;

public class IncidentsModel : PageModel
{
    private readonly IncidentsService _service;

    public List<Incident> Incidents { get; set; } = new();

    [BindProperty]
    public IncidentCreateDto Incident { get; set; } = new();

    public IncidentsModel(IncidentsService service)
    {
        _service = service;
    }

    public async Task OnGet()
    {
        Incidents = await _service.GetIncidents() ?? new List<Incident>();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            await OnGet();
            return Page();
        }

        await _service.CreateIncident(Incident);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostClose(int closeId)
    {
        await _service.CloseIncident(closeId);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDelete(int deleteId)
    {
        await _service.DeleteIncident(deleteId);
        return RedirectToPage();
    }
}