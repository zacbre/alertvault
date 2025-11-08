using AlertVault.Core.Repository;

namespace AlertVault.Core.Service;

public class AlertService(IAlertRepository alertRepository)
{
    public async Task<List<Alert>> All() => await alertRepository.All();
    
    public async Task<Alert?> Get(string uuid) => await alertRepository.Get(uuid);
    
    public async Task<Alert> Add(TimeSpan interval)
    {
        var alert = new Alert
        {
            UserId = 1,
            CreatedUtc = DateTime.UtcNow,
            Uuid = Guid.NewGuid().ToString(),
            Interval = interval,
            LastCheckUtc = DateTime.UtcNow
        };

        await alertRepository.Add(alert);

        return alert;
    }

    public async Task<Alert?> UpdateLastChecked(string uuid)
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

    public async Task<List<Request>> GetRequests(string uuid)
    {
        var alert = await alertRepository.Get(uuid);
        if (alert is null)
        {
            return null;
        }

        //return alert.Requests;
        return new List<Request>();
    }
}