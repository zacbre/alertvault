using System.Net.Http.Json;
using AlertVault.Core.Entities;
using AlertVault.Core.Entities.Dto;

namespace AlertVault.Core.Notifiers;

public class DiscordNotifier(HttpClient client) : IAlertNotifier
{
    public UserCredentialTypeEnum CredentialType => UserCredentialTypeEnum.DiscordWebhook;
    
    public async Task Notify(UserCredentials credentials, Alert alert)
    {
        var discordCredentials = credentials.Credentials.DiscordWebhook;
        if (discordCredentials == null)
        {
            throw new InvalidOperationException("Discord credentials are missing.");
        }

        var discordModel = new DiscordWebhookModel
        {
            Username = "AlertVault",
            Content = $"Alert Triggered: {alert.Name}\nDescription: {alert.Description}"
        };
        
        await client.PostAsJsonAsync(discordCredentials.Url, discordModel);
    }
}