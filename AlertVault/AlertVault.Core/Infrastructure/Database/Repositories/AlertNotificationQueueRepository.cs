using AlertVault.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlertVault.Core.Infrastructure.Database.Repositories;

public class AlertNotificationQueueRepository(DatabaseContext context) : BaseRepository(context)
{
    public async Task<AlertNotification?> Next()
    {
        var alertNotification = await context.AlertNotification.OrderBy(an => an.Id).Take(1).FirstOrDefaultAsync();
        if (alertNotification is null) return alertNotification;

        context.AlertNotification.Remove(alertNotification);
        await Save();

        return alertNotification;
    }

    public async Task Add(AlertNotification alertNotification)
    {
        var added = await context.AlertNotification.AddAsync(alertNotification);
        await Save();
        alertNotification.Id = added.Entity.Id;
    }
}