namespace AlertVault.Core.Repository;

public interface IAlertRepository
{
    Task<List<Alert>> All();
    Task<Alert?> Get(string uuid);
    Task<List<Request>> GetRequests(string uuid);
    Task Add(Alert alert);
    Task Update();
}