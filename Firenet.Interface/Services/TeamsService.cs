using System.Net.Http.Json;
using Firenet.Interface.Models;

namespace Firenet.Interface.Services;

public class TeamsService
{
    private readonly HttpClient _http;

    public TeamsService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("FirenetApi");
    }

    public Task<List<Team>?> GetTeams()
        => _http.GetFromJsonAsync<List<Team>>("api/teams");

    public Task<Team?> GetTeam(int id)
        => _http.GetFromJsonAsync<Team>($"api/teams/{id}");

    public Task<HttpResponseMessage> CreateTeam(Team team)
        => _http.PostAsJsonAsync("api/teams", team);

    public Task<HttpResponseMessage> UpdateTeam(Team team)
        => _http.PutAsJsonAsync($"api/teams/{team.Id}", team);

    public Task<HttpResponseMessage> DeleteTeam(int id)
        => _http.DeleteAsync($"api/teams/{id}");
}