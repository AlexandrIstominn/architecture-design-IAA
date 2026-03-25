using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace BotDetect.Api.Tests;

public class AccountsApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AccountsApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task POST_accounts_returns_201_and_account()
    {
        var body = new { username = "newuser", socialNetwork = "vk" };
        var response = await _client.PostAsJsonAsync("/api/accounts", body);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(json.TryGetProperty("accountId", out _));
        Assert.True(json.TryGetProperty("username", out _));
        Assert.True(response.Headers.Location?.ToString().Contains("/api/accounts/", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GET_accounts_id_returns_200_or_404()
    {
        var response = await _client.GetAsync("/api/accounts/1");
        Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.True(json.TryGetProperty("accountId", out _));
            Assert.True(json.TryGetProperty("username", out _));
        }
    }
}
