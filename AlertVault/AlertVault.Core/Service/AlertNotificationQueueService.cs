using AlertVault.Core.Entities;
using AlertVault.Core.Repository;

namespace AlertVault.Core.Service;

public class AlertNotificationQueueService(IAlertNotificationQueueRepository alertNotificationQueueRepository)
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