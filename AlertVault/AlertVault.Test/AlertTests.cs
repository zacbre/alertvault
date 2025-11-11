using System.Net.Http.Json;
using AlertVault.Core.Entities;
using AlertVault.Models;

namespace AlertVault.Test;

[Collection("Database")]
public class AlertTests(CustomWebApplicationFactory factory, Fixture fixture) : BaseTest(fixture)
{
    [Fact]
    public async Task Can_Make_Alert()
    {
        var client = factory.CreateClient();
        var alertModel = new AlertModel
        {
            Interval = TimeSpan.FromMinutes(5),
            UserId = (await CreateUser()).Id
        };
        var response = await client.PostAsJsonAsync("/api/alert", alertModel);
        response.EnsureSuccessStatusCode();
        var createdAlert = await response.Content.ReadFromJsonAsync<Alert>();
         Assert.NotNull(createdAlert);
         Assert.Equal(alertModel.Interval, createdAlert.Interval);
         Assert.Equal(alertModel.UserId, createdAlert.UserId);
    }
}