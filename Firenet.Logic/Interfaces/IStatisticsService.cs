public interface IStatisticsService
{
    Task<IEnumerable<int>> GetOnDutyCarIdsAsync();
    Task<IEnumerable<FirefighterIncidentCountDto>> GetFirefighterIncidentCountsLastYearAsync();
    Task<IncidentTimeStatsDto> GetIncidentTimeStatsLastYearAsync();
    Task<IEnumerable<CarEfficiencyDto>> GetCarEfficiencyByYearAsync();
}