using Api.Domain.Items;
using Api.Features.BestStories;
using Api.Infrastructure.ExternalApi;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Infrastructure.Cache;

/// <summary>
/// Cache service
/// </summary>
public class CacheService(
    IMemoryCache memoryCache,
    IHackerNewsApi hackerNewsApi,
    ILogger<CacheService> logger)
    : ICacheService
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    /// <summary>
    /// Get story from cache or from api
    /// </summary>
    /// <param name="storyId">Story id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="skipCache">Skip the cache and get the story from the API (for the updated stories)</param>
    /// <returns>Searched item</returns>
    public async Task<Item?> GetStoryFromCacheOrFromApiAsync(
        int storyId,
        CancellationToken cancellationToken,
        bool skipCache = false)
    {
        if (!skipCache && memoryCache.TryGetValue(storyId, out Item? story))
        {
            logger.LogInformation("Item {StoryId} found in cache", storyId);
            return story!;
        }

        try
        {
            await Semaphore.WaitAsync(cancellationToken);

            if (skipCache || !memoryCache.TryGetValue(storyId, out story))
            {
                story = await hackerNewsApi.GetItemByIdAsync(storyId, cancellationToken);
                memoryCache.Set(storyId, story);
            }
        }
        finally
        {
            Semaphore.Release();
        }

        return story;
    }
}