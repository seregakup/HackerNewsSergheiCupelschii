using System.Net;
using System.Text.Json;
using HackerNews.Api.Features.BestStories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HackerNews.Tests.GetBestStoriesTests;

public class BestStoriesTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private const string BaseUrl = "https://localhost:7091/api/news-management/best-news";
    private readonly HttpClient _client = factory.CreateClient();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    [Fact]
    public async Task Should_Return_Correct_Number_Of_Best_Stories()
    {
        // Arrange
        var query = new GetBestStories.Query { AmountOfItems = 5 };
        var queryString = new QueryString("?amountOfItems=5");
        var url = BaseUrl + queryString;

        // Act
        var response = await _client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<GetBestStories.BestStoriesResponse>>(content,_jsonOptions);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(query.AmountOfItems, result.Count);
    }
    
    [Fact]
    public async Task Should_Return_Best_Stories_In_Descending_Order_By_Score()
    {
        // Arrange
        var queryString = new QueryString("?amountOfItems=5");
        var url = BaseUrl + queryString;

        // Act
        var response = await _client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<GetBestStories.BestStoriesResponse>>(content, _jsonOptions);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.SequenceEqual(result.OrderByDescending(m => m.Score)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    [InlineData(-1)]
    public async Task Should_Throw_Exception_When_AmountOfItems_Is_Negative(int amountOfItems)
    {
        // Arrange
        var queryString = new QueryString($"?amountOfItems={amountOfItems}");
        var url = BaseUrl + queryString;

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}