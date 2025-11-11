using AlertVault.Core.Entities;
using AlertVault.Core.Repository;

namespace AlertVault.Core.Service;

public class AlertService(IAlertRepository alertRepository)
{
    public async Task<List<Alert>> All() => await alertRepository.All();
    
    public async Task<Alert?> Get(Guid uuid) => await alertRepository.Get(uuid);
    
    public async Task<Alert> Add(int userId, TimeSpan interval)
    {
        var alert = new Alert
        {
            UserId = userId,
            CreatedUtc = DateTime.UtcNow,
            Uuid = Guid.NewGuid(),
            Interval = interval,
            LastCheckUtc = DateTime.UtcNow
        };

        await alertRepository.Add(alert);

        return alert;
    }

    public async Task<Alert?> UpdateLastChecked(Guid uuid)
    {
        var alert = await alertRepository.Get(uuid);
        if (alert is null)
        {
            return null;
        }

        alert.LastCheckUtc = DateTime.UtcNow;
        await alertRepository.Update();
        
        return alert;
    }

    public async Task<List<Request>> GetRequests(Guid uuid)
    {
        var alert = await alertRepository.Get(uuid);
        if (alert is null)
        {
            return [];
        }

        return alert.Requests;
    }
}