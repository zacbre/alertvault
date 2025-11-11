using AlertVault.Core.Entities;

namespace AlertVault.Core.Repository;

public interface IAlertNotificationQueueRepository
{
    Task<AlertNotification?> Next();
    Task Add(AlertNotification alertNotification);
    Task Save();
}