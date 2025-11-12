using AlertVault.Core.Entities;

namespace AlertVault.Core.Test.Tests;

[Collection("Database")]
public class UserCredentialsTests(Fixture fixture) : BaseTest(fixture)
{
    [Fact]
    public async Task Can_Create_User_Credentials()
    {
        var user = await CreateUser();
        var credentials = new Entities.UserCredentials
        {
            UserId = user.Id,
            CredentialType = Entities.Dto.UserCredentialTypeEnum.DiscordWebhook,
            Credentials = new CredentialHelper
            {
                DiscordWebhook = new DiscordWebhookCredentials
                {
                    Url = "https://discord.com/api/webhooks/your_webhook_url"
                } 
            }
        };

        await UserCredentialsService.Add(credentials);
        var retrievedCredentials = await UserCredentialsService.GetByUserId(user.Id);
        Assert.NotNull(retrievedCredentials);
        Assert.Single(retrievedCredentials);
        Assert.Equal(user.Id, retrievedCredentials[0].UserId);
        Assert.Equal(Entities.Dto.UserCredentialTypeEnum.DiscordWebhook, retrievedCredentials[0].CredentialType);
        Assert.NotNull(retrievedCredentials[0].Credentials.DiscordWebhook);
        Assert.Equal("https://discord.com/api/webhooks/your_webhook_url", retrievedCredentials[0].Credentials.DiscordWebhook!.Url);
    }
}