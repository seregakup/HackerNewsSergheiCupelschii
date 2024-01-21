using System.Globalization;
using Api.Features.BestStories;
using Swashbuckle.AspNetCore.Filters;

namespace Api.ResponseExamples;

/// <summary>
/// Message body for ok
/// </summary>
public class MessageBodyOkExample : IExamplesProvider<GetBestStories.BestStoriesResponse>
{
    /// <summary>
    /// Get examples
    /// </summary>
    /// <returns>Ok response</returns>
    public GetBestStories.BestStoriesResponse GetExamples()
    {
        return new GetBestStories.BestStoriesResponse
        {
            PostedBy = "dhouston",
            CommentCount = 16,
            Score = 111,
            Time = DateTime.ParseExact("2021-10-10T10:10:10+00:00", "yyyy-MM-ddTHH:mm:ss+00:00", CultureInfo.InvariantCulture),
            Title = "Some text",
            Uri = new Uri("https://www.google.com")
        };
    }
}