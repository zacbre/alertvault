using AlertVault.Core.Entities;
using AlertVault.Core.Entities.Dto;

namespace AlertVault.Core.Notifiers;

public interface IAlertNotifier
{
    UserCredentialTypeEnum CredentialType { get; }
    Task Notify(UserCredentials credentials, Alert alert);
}