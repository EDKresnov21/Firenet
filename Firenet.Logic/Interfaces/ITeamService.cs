public interface ITeamService
{
    Task ResetAndArrangeCurrentTeamsAsync();
    Task RearrangeTeamsOnChangeAsync(int? removedCarId = null, int? removedFighterId = null);
}