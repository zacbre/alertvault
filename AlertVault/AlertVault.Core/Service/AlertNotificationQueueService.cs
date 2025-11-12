using AlertVault.Core.Entities;
using AlertVault.Core.Infrastructure.Database;
using AlertVault.Core.Infrastructure.Database.Repositories;

namespace AlertVault.Core.Service;

public class AlertNotificationQueueService(AlertNotificationQueueRepository alertNotificationQueueRepository)
{
    public Task Push(AlertNotification alertNotification)
    {
        return alertNotificationQueueRepository.Add(alertNotification);
    }

    public async Task<AlertNotification?> Pop()
    {
        return await alertNotificationQueueRepository.Next();
    }
}