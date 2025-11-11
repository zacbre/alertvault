using AlertVault.Core.Entities;
using AlertVault.Core.Service;

namespace AlertVault.Core.Jobs;

public class FindExpiredAlertsJob(AlertService alertService, AlertNotificationQueueService alertNotificationQueueService) : IJobInstance
{
    public async Task Execute()
    {
        var alerts = await alertService.AllExpired();
        foreach (var alert in alerts)
        {
            var alertNotification = new AlertNotification { AlertId = alert.Id };
            await alertNotificationQueueService.Push(alertNotification);
            
            alert.LastReported = DateTime.UtcNow;
            await alertService.Save();
        }
    }
}