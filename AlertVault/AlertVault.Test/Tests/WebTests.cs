using AlertVault.Core.Test;

namespace AlertVault.Test.Tests;

[Collection("Database")]
public class WebTests(CustomWebApplicationFactory factory, Fixture fixture) : CustomBaseTest(fixture)
{
    [Fact]
    public async Task Can_Request_Index()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/");
        response.EnsureSuccessStatusCode();
    }
}