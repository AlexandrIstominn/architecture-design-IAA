using System.Net;
using System.Text.Json;
using Xunit;

namespace BotDetect.Api.Tests;

public class ReportsApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ReportsApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Add("X-User-Id", "1");
    }

    [Fact]
    public async Task GET_reports_returns_200_and_array()
    {
        var response = await _client.GetAsync("/api/reports");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var arr = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(JsonValueKind.Array, arr.ValueKind);
    }
}
