using AlertVault.Core.Service;

namespace AlertVault.Core.Test;

[Collection("Database")]
public class AlertTests(Fixture fixture) : BaseTest(fixture)
{
    [Fact]
    public async Task Can_Make_Alert()
    {
        var user = await CreateUser();
        var alert = await AlertService.Add(user.Id, TimeSpan.FromMinutes(5));

        var gottenAlert = await AlertService.Get(alert.Uuid);
        Assert.NotNull(gottenAlert);
        Assert.Equal(alert.Id, gottenAlert.Id);
        Assert.Equal(alert.Interval, gottenAlert.Interval);
        Assert.Equal(alert.Uuid, gottenAlert.Uuid);
        Assert.Equal(alert.UserId, gottenAlert.UserId);
    }

    [Fact]
    public async Task Can_Update_LastChecked()
    {
        var user = await CreateUser();
        var alert = await AlertService.Add(user.Id, TimeSpan.FromMinutes(5));
        var originalDateTime = alert.LastCheckUtc;
        // Delay to ensure LastCheckUtc will be different
        await Task.Delay(1000);
        var updatedAlert = await AlertService.UpdateLastChecked(alert.Uuid);
        Assert.NotNull(updatedAlert);
        
        Assert.NotNull(updatedAlert);
        Assert.Equal(alert.Id, updatedAlert.Id);
        Assert.Equal(alert.Interval, updatedAlert.Interval);
        Assert.Equal(alert.Uuid, updatedAlert.Uuid);
        Assert.Equal(alert.UserId, updatedAlert.UserId);
        Assert.NotEqual(originalDateTime, updatedAlert.LastCheckUtc);
        Assert.True(originalDateTime < alert.LastCheckUtc);
    }
}