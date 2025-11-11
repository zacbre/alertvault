using AlertVault.Core.Entities;
using AlertVault.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace AlertVault.Db;

public class AlertNotificationQueueRepository(DatabaseContext context) : BaseRepository(context)
{
    public async Task<AlertNotification?> Next()
    {
        var alertNotification = await context.AlertNotifications.OrderBy(an => an.Id).Take(1).FirstOrDefaultAsync();
        if (alertNotification is null) return alertNotification;
        
        context.AlertNotifications.Remove(alertNotification);
        await Save();

        return alertNotification;
}

    public async Task Add(AlertNotification alertNotification)
    {
        var added = await context.AlertNotifications.AddAsync(alertNotification);
        await Save();
        alertNotification.Id = added.Entity.Id;
    }
}