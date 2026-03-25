using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace BotDetect.Api.Tests;

public class AnalysisApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AnalysisApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Add("X-User-Id", "1");
    }

    [Fact]
    public async Task POST_analysis_returns_201_and_historyId()
    {
        var body = new { socialNetwork = "vk", accountIds = new[] { "u1", "u2" } };
        var response = await _client.PostAsJsonAsync("/api/analysis", body);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(json.TryGetProperty("historyId", out _));
        Assert.True(response.Headers.Location?.ToString().Contains("/api/analysis/", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GET_analysis_id_returns_200_with_summary_or_404()
    {
        var response = await _client.GetAsync("/api/analysis/99999");
        Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.True(json.TryGetProperty("historyId", out _));
            Assert.True(json.TryGetProperty("status", out _));
            Assert.True(json.TryGetProperty("results", out _));
        }
    }

    [Fact]
    public async Task GET_analysis_returns_200_and_array()
    {
        var response = await _client.GetAsync("/api/analysis");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var arr = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(JsonValueKind.Array, arr.ValueKind);
    }
}
