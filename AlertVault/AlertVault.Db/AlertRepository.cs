using AlertVault.Core;
using AlertVault.Core.Entities;
using AlertVault.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace AlertVault.Db;

public class AlertRepository(DatabaseContext context) : BaseRepository(context)
{
    public async Task<List<Alert>> All() => await context.Alert.ToListAsync();

    public async Task<List<Alert>> AllExpired()
    {
        var query = (from a in context.Alert
            where a.LastCheckUtc + a.Interval < DateTime.UtcNow &&
                  (a.LastReported == null || a.LastReported + TimeSpan.FromMinutes(15) < DateTime.UtcNow)
            select a);
        
        return await query.ToListAsync();
    }

    public async Task<Alert?> Get(Guid uuid) =>
        await (from alert in context.Alert
            where alert.Uuid == uuid
            orderby alert.Id descending
            select alert).FirstOrDefaultAsync();

    public Task<List<Request>> GetRequests(Guid uuid)
    {
        return Task.FromResult(new List<Request>());
    }

    public async Task Add(Alert alert)
    {
        var addedAlert = await context.Alert.AddAsync(alert);
        await Save();
        alert.Id = addedAlert.Entity.Id;
    }

    public Task Delete(Alert alert)
    {
        context.Alert.Remove(alert);
        return Save();
    }
}