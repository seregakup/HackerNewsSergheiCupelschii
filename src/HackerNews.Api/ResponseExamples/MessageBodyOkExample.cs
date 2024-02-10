using System.Globalization;
using HackerNews.Api.Features.BestStories;
using Swashbuckle.AspNetCore.Filters;

namespace HackerNews.Api.ResponseExamples;

/// <summary>
/// Message body for ok
/// </summary>
public class MessageBodyOkExample : IExamplesProvider<IReadOnlyList<GetBestStories.BestStoriesResponse>>
{
    /// <summary>
    /// Get examples
    /// </summary>
    /// <returns>Ok response</returns>
    public IReadOnlyList<GetBestStories.BestStoriesResponse> GetExamples()
    {
        return
        [
            new GetBestStories.BestStoriesResponse
            {
                PostedBy = "dhouston",
                CommentCount = 16,
                Score = 111,
                Time = DateTime.Parse("2021-10-10T10:10:10+00:00", CultureInfo.InvariantCulture),
                Title = "Some text"
            }
        ];
    }
}