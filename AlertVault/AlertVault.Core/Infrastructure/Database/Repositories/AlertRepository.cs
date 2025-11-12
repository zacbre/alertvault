using AlertVault.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlertVault.Core.Infrastructure.Database.Repositories;

public class AlertRepository(DatabaseContext context) : BaseRepository(context)
{
    public async Task<List<Alert>> All()
    {
        return await context.Alert.ToListAsync();
    }

    public async Task<List<Alert>> AllExpired()
    {
        var query = from a in context.Alert
            where a.LastCheckUtc + a.Interval < DateTime.UtcNow &&
                  (a.LastReported == null || a.LastReported + TimeSpan.FromMinutes(15) < DateTime.UtcNow)
            select a;

        return await query.ToListAsync();
    }

    public async Task<Alert?> Get(Guid uuid)
    {
        return await (from alert in context.Alert
            where alert.Uuid == uuid
            orderby alert.Id descending
            select alert).FirstOrDefaultAsync();
    }

    public Task<List<Request>> GetRequests(Guid uuid)
    {
        var query = from alert in context.Alert
            where alert.Uuid == uuid
            join request in context.Request on alert.Id equals request.AlertId
            select request;

        return query.Include(p => p.UserAgent).ToListAsync();
    }

    public async Task Add(Alert alert)
    {
        var addedAlert = await context.Alert.AddAsync(alert);
        await Save();
        alert.Id = addedAlert.Entity.Id;
    }
    
    public async Task AddRequest(Request request)
    {
        var addedRequest = await context.Request.AddAsync(request);
        await Save();
        request.Id = addedRequest.Entity.Id;
    }

    public Task Delete(Alert alert)
    {
        context.Alert.Remove(alert);
        return Save();
    }
}