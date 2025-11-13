namespace AlertVault.Core.Test.Tests;

[Collection("Database")]
public class AlertTests(Fixture fixture) : BaseTest(fixture)
{
    [Fact]
    public async Task Can_Make_Alert()
    {
        var user = await CreateUser();
        Assert.NotNull(user);
        var pulledAlert = await AlertService.Add(user.Id, TimeSpan.FromMinutes(5));
        Assert.True(pulledAlert.IsSuccess);
        Assert.NotNull(pulledAlert.Value);

        var alert = pulledAlert.Value;

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
        Assert.NotNull(user);
        var addedAlert = await AlertService.Add(user.Id, TimeSpan.FromMinutes(5));
        Assert.True(addedAlert.IsSuccess);
        Assert.NotNull(addedAlert.Value);
        var alert = addedAlert.Value;
        
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