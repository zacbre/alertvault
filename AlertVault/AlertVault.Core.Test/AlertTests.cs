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

        var firstAlert = await AlertService.Get(alert.Uuid);
        Assert.NotNull(firstAlert);
        Assert.Equal(alert.Id, firstAlert.Id);
        Assert.Equal(alert.Interval, firstAlert.Interval);
        Assert.Equal(alert.Uuid, firstAlert.Uuid);
        Assert.Equal(alert.UserId, firstAlert.UserId);

        DatabaseContext.Alert.Update(alert);
        await DatabaseContext.SaveChangesAsync();
        
        var gottenAlert = await AlertService.Get(alert.Uuid);
        Assert.NotNull(gottenAlert);
        Assert.NotEqual(alert.UpdatedUtc, gottenAlert.CreatedUtc);
    }
}