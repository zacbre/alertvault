using System.Net.Http.Json;
using System.Threading.Tasks;
using AlertVault.Core.Entities;
using AlertVault.Core.Test;
using Xunit;

namespace AlertVault.Test.Tests;

[Collection("Database")]
public class UserLoginTokenTests(CustomWebApplicationFactory factory, Fixture fixture) : CustomBaseTest(fixture)
{
    [Fact]
    public async Task Login_Returns_Token_And_Saves_To_Db()
    {
        var client = factory.CreateClient();
        var rawPassword = "1234";
        var user = await UserService.Add(new User { Email = "test@test.com", Password = BCrypt.Net.BCrypt.HashPassword(rawPassword) });
        var loginModel = new { email = user.Email, password = rawPassword };
        var response = await client.PostAsJsonAsync("/api/user/login", loginModel);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(result);

        // Check token exists in DB
        var tokenInDb = await UserService.ValidateToken(result.Token);
        Assert.NotNull(tokenInDb);
        Assert.Equal(user.Id, tokenInDb.UserId);
        Assert.Equal(result.Token, tokenInDb.Token);
    }

    [Fact]
    public async Task Invalid_Login_Returns_Unauthorized()
    {
        var client = factory.CreateClient();
        var loginModel = new { email = "wrong@test.com", password = "wrongpass" };
        var response = await client.PostAsJsonAsync("/api/user/login", loginModel);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Token_Authentication_Middleware_Sets_UserId()
    {
        var client = factory.CreateClient();
        var rawPassword = "1234";
        var user = await UserService.Add(new User { Email = "test2@test.com", Password = BCrypt.Net.BCrypt.HashPassword(rawPassword) });
        var loginModel = new { email = user.Email, password = rawPassword };
        var loginResponse = await client.PostAsJsonAsync("/api/user/login", loginModel);
        var result = await loginResponse.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(result);
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token.ToString());
        var response = await client.GetAsync("/api/user/me");
        response.EnsureSuccessStatusCode();
        var meResult = await response.Content.ReadFromJsonAsync<UserToken>();
        Assert.NotNull(meResult);
        Assert.Equal(user.Id, meResult.UserId);
        Assert.Equal(result.Token, meResult.Token);
    }

    private class TokenResponse
    {
        public Guid Token { get; set; }
    }
}
