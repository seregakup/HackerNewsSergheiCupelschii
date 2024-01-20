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
            By = "dhouston",
            Descendants = 16,
            Id = 8863,
            Kids = [2922097, 2922429, 2924562],
            Score = 111,
            Text = "Some text",
            Time = 1314211127,
            Title = "Some text",
            Type = "story"
        };
    }
}