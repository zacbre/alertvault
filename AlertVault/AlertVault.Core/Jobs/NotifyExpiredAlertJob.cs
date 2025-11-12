using AlertVault.Core.Entities;
using AlertVault.Core.Notifiers;
using AlertVault.Core.Service;

namespace AlertVault.Core.Jobs;

public class NotifyExpiredAlertJob(AlertNotificationQueueService alertNotificationQueueService, IEnumerable<IAlertNotifier> alertNotifiers)
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