using Api.Features.BestStories;

namespace Api.Services;

/// <summary>
/// Interface for the Hacker News Service
/// </summary>
public interface IHackerNewsService
{
    /// <summary>
    /// Returns the best news sorted by score
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A collection of sorted best news</returns>
    Task<IReadOnlyList<GetBestStories.BestStoriesResponse>> GetSortedBestNewsByScoreAsync(CancellationToken cancellationToken);
}