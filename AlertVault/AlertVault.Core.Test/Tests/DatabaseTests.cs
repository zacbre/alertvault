namespace AlertVault.Core.Test.Tests;

[Collection("Database")]
public class DatabaseTests(Fixture fixture) : BaseTest(fixture)
{
    [Fact]
    public async Task Database_Can_Update_Updated_Utc()
    {
        var user = await CreateUser();
        var addedAlert = await AlertService.Add(user.Id, TimeSpan.FromMinutes(5));
        Assert.True(addedAlert.IsSuccess);
        Assert.NotNull(addedAlert.Value);
        
        var alert = addedAlert.Value;
        
        var originalDateTime = alert.UpdatedUtc;

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
        Assert.NotEqual(originalDateTime, gottenAlert.UpdatedUtc);
        Assert.True(originalDateTime < gottenAlert.UpdatedUtc);
    }
}