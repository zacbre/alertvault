using AlertVault.Core.Entities;
using AlertVault.Core.Infrastructure.Database.Repositories;
using AlertVault.Core.Notifiers;
using AlertVault.Core.Service;

namespace AlertVault.Core.Jobs;

public class NotifyExpiredAlertJob(AlertNotificationQueueService alertNotificationQueueService, UserCredentialsService userCredentialsService, IEnumerable<IAlertNotifier> alertNotifiers)
{
    public async Task Execute()
    {
        AlertNotification? alertNotification = null;
        while ((alertNotification = await alertNotificationQueueService.Pop()) != null)
        {
            var alert = alertNotification.Alert;
            var enabledNotifiers = alert.AlertConfiguration?.EnabledNotifiers;
            if (enabledNotifiers is null or { Count: 0 })
            {
                return;
            }

            var credentials = await userCredentialsService.GetByUserId(alert.UserId);
            var notifiersToRun = alertNotifiers.Where(p => enabledNotifiers.Contains(p.CredentialType)).ToList();
            foreach (var notifier in notifiersToRun)
            {
                // Make sure we have the credential for this particular entry.
                var credential = credentials.FirstOrDefault(p => p.CredentialType == notifier.CredentialType);
                if (credential is null)
                {
                    // Need to alert the user later they don't have these creds? This shouldn't be possible theoretically.
                    continue;
                }

                await notifier.Notify(credential, alert);
            }
        }
    }
}