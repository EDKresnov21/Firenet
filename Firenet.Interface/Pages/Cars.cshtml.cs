using Firenet.Interface.Models;
using Firenet.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Firenet.Interface.Pages;

public class CarsModel : PageModel
{
    private readonly CarsService _service;

    public List<Car> Cars { get; set; } = new();

    [BindProperty]
    public Car Car { get; set; } = new();

    public bool IsEditing { get; set; }

    public CarsModel(CarsService service)
    {
        _service = service;
    }

    public async Task OnGet()
    {
        Cars = await _service.GetCars() ?? new List<Car>();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            await OnGet();
            return Page();
        }

        if (Car.Id == 0)
            await _service.CreateCar(Car);
        else
            await _service.UpdateCar(Car);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEdit(int editId)
    {
        Cars = await _service.GetCars() ?? new List<Car>();

        var car = Cars.FirstOrDefault(c => c.Id == editId);
        if (car == null)
            return RedirectToPage();

        ModelState.Clear();

        Car = car;
        IsEditing = true;

        return Page();
    }

    public async Task<IActionResult> OnPostDelete(int deleteId)
    {
        await _service.DeleteCar(deleteId);
        return RedirectToPage();
    }
}