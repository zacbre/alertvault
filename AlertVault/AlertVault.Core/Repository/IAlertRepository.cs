using AlertVault.Core.Entities;

namespace AlertVault.Core.Repository;

public interface IAlertRepository
{
    Task<List<Alert>> All();
    Task<List<Alert>> AllExpired();
    Task<Alert?> Get(Guid uuid);
    Task<List<Request>> GetRequests(Guid uuid);
    Task Add(Alert alert);
    Task Delete(Alert alert);
    Task Save();
    
}