using Microsoft.AspNetCore.Mvc.Testing;

namespace AlertVault.Test;

[Collection("Database")]

public class WebTests(CustomWebApplicationFactory factory, Fixture fixture) : BaseTest(fixture)
{
    [Fact]
    public async Task Can_Request_Index()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/");
        response.EnsureSuccessStatusCode();
    }
}