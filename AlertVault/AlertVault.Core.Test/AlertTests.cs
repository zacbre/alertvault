using AlertVault.Core.Service;

namespace AlertVault.Core.Test;

[Collection("Database")]
public class AlertTests(Fixture fixture) : BaseTest(fixture)
{
    [Fact]
    public async Task Can_Make_Alert()
    {
        var user = await CreateUser();
        var alert = await AlertService.Add(TimeSpan.FromMinutes(5));
        
        var alerts = await AlertService.All();
        Assert.Single(alerts);
        Assert.Equal(alert.Id, alerts[0].Id);
        Assert.Equal(alert.Interval, alerts[0].Interval);
        Assert.Equal(alert.Uuid, alerts[0].Uuid);
        Assert.Equal(alert.UserId, alerts[0].UserId);
    }
}