public interface IIncidentService
{
    Task<Incident> CreateIncidentAsync(IncidentCreateDto dto);
    Task CloseIncidentAsync(int incidentId);
}