using Firenet.Interface.Models;
using Firenet.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Firenet.Interface.Pages;

public class FirefightersModel : PageModel
{
    private readonly FirefightersService _service;

    public List<Firefighter> Firefighters { get; set; } = new();

    [BindProperty]
    public Firefighter Firefighter { get; set; } = new();

    public bool IsEditing { get; set; }

    public FirefightersModel(FirefightersService service)
    {
        _service = service;
    }

    public async Task OnGet()
    {
        Firefighters = await _service.GetFirefighters() ?? new List<Firefighter>();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            await OnGet();
            return Page();
        }

        if (Firefighter.Id == 0)
            await _service.CreateFirefighter(Firefighter);
        else
            await _service.UpdateFirefighter(Firefighter);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEdit(int editId)
    {
        Firefighters = await _service.GetFirefighters() ?? new List<Firefighter>();

        var f = await _service.GetFirefighter(editId);
        if (f == null) return RedirectToPage();

        Firefighter = f;
        IsEditing = true;

        return Page();
    }

    public async Task<IActionResult> OnPostDelete(int deleteId)
    {
        await _service.DeleteFirefighter(deleteId);
        return RedirectToPage();
    }
}