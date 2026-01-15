using System.Net.Http.Json;
using Firenet.Interface.Models;

namespace Firenet.Interface.Services;

public class FirefightersService
{
    private readonly HttpClient _http;

    public FirefightersService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("FirenetApi");
    }

    public Task<List<Firefighter>?> GetFirefighters()
        => _http.GetFromJsonAsync<List<Firefighter>>("api/firefighters");

    public Task<Firefighter?> GetFirefighter(int id)
        => _http.GetFromJsonAsync<Firefighter>($"api/firefighters/{id}");

    public Task<HttpResponseMessage> CreateFirefighter(Firefighter firefighter)
        => _http.PostAsJsonAsync("api/firefighters", firefighter);

    public Task<HttpResponseMessage> UpdateFirefighter(Firefighter firefighter)
        => _http.PutAsJsonAsync($"api/firefighters/{firefighter.Id}", firefighter);

    public Task<HttpResponseMessage> DeleteFirefighter(int id)
        => _http.DeleteAsync($"api/firefighters/{id}");
}