using System.Net.Http.Json;
using Firenet.Interface.Models;

namespace Firenet.Interface.Services;

public class IncidentsService
{
    private readonly HttpClient _http;

    public IncidentsService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("FirenetApi");
    }

    public Task<List<Incident>?> GetIncidents()
        => _http.GetFromJsonAsync<List<Incident>>("api/incidents");

    public Task<Incident?> GetIncident(int id)
        => _http.GetFromJsonAsync<Incident>($"api/incidents/{id}");

    public Task<HttpResponseMessage> CreateIncident(IncidentCreateDto dto)
        => _http.PostAsJsonAsync("api/incidents", dto);

    public Task<HttpResponseMessage> UpdateIncident(Incident incident)
        => _http.PutAsJsonAsync($"api/incidents/{incident.Id}", incident);

    public Task<HttpResponseMessage> DeleteIncident(int id)
        => _http.DeleteAsync($"api/incidents/{id}");

    public Task<HttpResponseMessage> CloseIncident(int id)
        => _http.PostAsync($"api/incidents/{id}/close", null);
}