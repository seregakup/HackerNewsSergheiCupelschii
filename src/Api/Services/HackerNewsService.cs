using Api.Features.BestStories;
using Api.Infrastructure.ExternalApi;
using AutoMapper;

namespace Api.Services;

/// <summary>
/// Service for the Hacker News API
/// </summary>
public class HackerNewsService(IHackerNewsApi hackerNewsApi, IMapper mapper) : IHackerNewsService
{
    
    /// <summary>
    /// Returns the best news sorted by score
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A collection of sorted best news</returns>
    public async Task<IReadOnlyList<GetBestStories.BestStoriesResponse>> GetSortedBestNewsByScoreAsync(CancellationToken cancellationToken)
    {
        var bestStories = await hackerNewsApi.GetBestStoriesAsync(cancellationToken);

        if (bestStories.Length == 0)
        {
            return new List<GetBestStories.BestStoriesResponse>(0);
        }
        
        var tasks = bestStories.Select(async storyId =>
        {
            var story = await hackerNewsApi.GetItemByIdAsync(storyId, cancellationToken);
            return story;
        }).ToList();
        
        var stories = await Task.WhenAll(tasks);
        
        var outputStories = mapper.Map<List<GetBestStories.BestStoriesResponse>>(stories);
        
        return outputStories.OrderByDescending(s => s.Score).ToList();
    }
}