using AlertVault.Core.Entities;
using AlertVault.Core.Service;

namespace AlertVault.Core.Jobs;

public class NotifyExpiredAlertJob(AlertNotificationQueueService alertNotificationQueueService) : IJobInstance
{
    public async Task Execute()
    {
        AlertNotification? alertNotification;
        while ((alertNotification = await alertNotificationQueueService.Pop()) != null)
        {
            // Here you would add the logic to notify about the expired alert.
            // For example, sending an email or a push notification.
            Console.WriteLine($"Notifying about expired alert with ID: {alertNotification.AlertId}");
        }
    }
}