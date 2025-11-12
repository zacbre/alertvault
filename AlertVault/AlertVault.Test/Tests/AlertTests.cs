using System.Net.Http.Json;
using AlertVault.Core.Entities;
using AlertVault.Core.Entities.Dto;
using AlertVault.Core.Test;
using AlertVault.Models;

namespace AlertVault.Test.Tests;

[Collection("Database")]
public class AlertTests(CustomWebApplicationFactory factory, Fixture fixture) : CustomBaseTest(fixture)
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

    [Fact]
    public async Task Alert_Creates_Request_Record()
    {
        var client = factory.CreateClient();
        var user = await CreateUser();
        var alertModel = new AlertModel
        {
            Interval = TimeSpan.FromMinutes(5),
            UserId = user.Id
        };
        
        var response = await client.PostAsJsonAsync("/api/alert", alertModel);
        response.EnsureSuccessStatusCode();
        var createdAlert = await response.Content.ReadFromJsonAsync<Alert>();
        Assert.NotNull(createdAlert);
        
        client.DefaultRequestHeaders.Add("User-Agent", "AlertVaultTestAgent/1.0");
        // Ping the alert.
        var pingResponse = await client.PostAsJsonAsync($"/{createdAlert.Uuid}", new {TestItem = "TestContent"});
        pingResponse.EnsureSuccessStatusCode();

        await Task.Delay(1000);

        var requests = await AlertService.GetRequests(createdAlert.Uuid);
        Assert.NotNull(requests);
        Assert.Single(requests);
        Assert.Equal("{\"testItem\":\"TestContent\"}", requests[0].Body);
        Assert.Equal(RequestMethodTypeEnum.POST, requests[0].Method);
        Assert.Equal("AlertVaultTestAgent/1.0", requests[0].UserAgent?.UserAgentString);
    }
    
    [Fact]
    public async Task Can_Update_LastChecked()
    {
        var client = factory.CreateClient();
        var user = await CreateUser();
        var alertModel = new AlertModel
        {
            Interval = TimeSpan.FromMinutes(5),
            UserId = user.Id
        };
        
        var response = await client.PostAsJsonAsync("/api/alert", alertModel);
        response.EnsureSuccessStatusCode();
        var createdAlert = await response.Content.ReadFromJsonAsync<Alert>();
        Assert.NotNull(createdAlert);
        
        var originalDateTime = createdAlert.LastCheckUtc;
        // Delay to ensure LastCheckUtc will be different
        await Task.Delay(1000);

        var updateResponse = await client.PutAsync($"/{createdAlert.Uuid}", null);
        updateResponse.EnsureSuccessStatusCode();
        var updatedAlert = await updateResponse.Content.ReadFromJsonAsync<Alert>();
        Assert.NotNull(updatedAlert);
        Assert.Equal(createdAlert.Uuid, updatedAlert.Uuid);
        Assert.NotEqual(originalDateTime, updatedAlert.LastCheckUtc);
        Assert.True(originalDateTime < updatedAlert.LastCheckUtc);
    }
    
    [Fact]
    public async Task Cannot_Create_Alert_With_Invalid_Data()
    {
        var client = factory.CreateClient();
        var alertModel = new AlertModel
        {
            Interval = TimeSpan.FromMinutes(-5),
        };
        
        var response = await client.PostAsJsonAsync("/api/alert", alertModel);
        Assert.False(response.IsSuccessStatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(errorResponse);
        Assert.Contains("Interval must be at least 1 minute.", errorResponse.Errors);
    }
}