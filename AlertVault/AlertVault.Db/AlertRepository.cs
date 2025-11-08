using AlertVault.Core;
using AlertVault.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace AlertVault.Db;

public class AlertRepository(DatabaseContext context) : IAlertRepository
{
    public async Task<List<Alert>> All() => await context.Alert.ToListAsync();

    public async Task<Alert?> Get(string uuid) =>
        await (from alert in context.Alert
            where alert.Uuid == uuid
            select alert).FirstOrDefaultAsync();

    public Task<List<Request>> GetRequests(string uuid)
    {
        return Task.FromResult(new List<Request>());
    }

    public async Task Add(Alert alert)
    {
        var addedAlert = await context.Alert.AddAsync(alert);
        await Update();
        alert.Id = addedAlert.Entity.Id;
    }

    public async Task Update()
    {
        await context.SaveChangesAsync();
    }
}