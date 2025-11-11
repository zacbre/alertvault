using AlertVault.Core.Entities;
using AlertVault.Core.Service;

namespace AlertVault.Core.Jobs;

public class NotifyExpiredAlertJob(AlertNotificationQueueService alertNotificationQueueService)
{
    public async Task Execute()
    {
        AlertNotification? alertNotification = null;
        while ((alertNotification = await alertNotificationQueueService.Pop()) != null)
        {
            // Get the alert notification details
        }
    }
}