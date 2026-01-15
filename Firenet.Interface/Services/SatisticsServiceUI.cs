using System.Net.Http.Json;
using Firenet.Interface.Models;

public class StatisticsServiceUI
{
    private readonly HttpClient _http;

    public StatisticsServiceUI(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("FirenetApi");
    }

    public Task<List<int>?> GetOnDutyCarIdsAsync()
        => _http.GetFromJsonAsync<List<int>>("api/cars/on-duty-ids");

    public Task<List<FirefighterIncidentCountDto>?> GetFirefighterIncidentCountsLastYearAsync()
        => _http.GetFromJsonAsync<List<FirefighterIncidentCountDto>>("api/statistics/firefighters-incidents-last-year");

    public Task<IncidentTimeStatsDto?> GetIncidentTimeStatsLastYearAsync()
        => _http.GetFromJsonAsync<IncidentTimeStatsDto>("api/statistics/incident-times-last-year");

    public Task<List<CarEfficiencyDto>?> GetCarEfficiencyByYearAsync()
        => _http.GetFromJsonAsync<List<CarEfficiencyDto>>("api/statistics/car-efficiency");
}