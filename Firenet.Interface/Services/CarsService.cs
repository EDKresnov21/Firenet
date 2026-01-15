using System.Net.Http.Json;
using Firenet.Interface.Models;

namespace Firenet.Interface.Services;

public class CarsService
{
    private readonly HttpClient _http;

    public CarsService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("FirenetApi");
    }

    public Task<List<Car>?> GetCars()
        => _http.GetFromJsonAsync<List<Car>>("api/cars");

    public Task<Car?> GetCar(int id)
        => _http.GetFromJsonAsync<Car>($"api/cars/{id}");

    public Task<HttpResponseMessage> CreateCar(Car car)
        => _http.PostAsJsonAsync("api/cars", car);

    public Task<HttpResponseMessage> UpdateCar(Car car)
        => _http.PutAsJsonAsync($"api/cars/{car.Id}", car);

    public Task<HttpResponseMessage> DeleteCar(int id)
        => _http.DeleteAsync($"api/cars/{id}");
}