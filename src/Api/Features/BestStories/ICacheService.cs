using Api.Domain.Items;

namespace Api.Features.BestStories;

/// <summary>
/// Interface for the cache service
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Get a story from the cache or from the Hacker News API
    /// </summary>
    /// <param name="storyId">Id of the story</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="skipCache">Skip the cache and get the story from the API (for the updated stories)</param>
    /// <returns>Story</returns>
    Task<Item?> GetStoryFromCacheOrFromApiAsync(int storyId, CancellationToken cancellationToken, bool skipCache = false);
}