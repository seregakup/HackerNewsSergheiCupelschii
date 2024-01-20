using System.Collections.Concurrent;
using Api.Domain.Items;
using Api.Infrastructure.ExternalApi;
using AutoMapper;

namespace Api.Features.BestStories;

/// <summary>
/// Service for the Hacker News API
/// </summary>
public class HackerNewsService(
    IHackerNewsApi hackerNewsApi, 
    IMapper mapper,
    ICacheService cacheService)
    : IHackerNewsService
{
    /// <summary>
    /// Returns the best news sorted by score
    /// </summary>
    /// <param name="amountOfItems">Amount of items to return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A collection of sorted best news</returns>
    public async Task<IReadOnlyList<GetBestStories.BestStoriesResponse>> GetSortedByScoreAmountOfBestNewsAsync(
        int amountOfItems,
        CancellationToken cancellationToken)
    {
        if (amountOfItems is < 1 or > 500)
        {
            throw new ArgumentOutOfRangeException(nameof(amountOfItems),
                message: "Amount of items must be between 1 and 500");
        }

        var bestStoriesIds = await hackerNewsApi.GetBestStoriesIdsAsync(cancellationToken);

        if (bestStoriesIds.Length == 0)
        {
            return new List<GetBestStories.BestStoriesResponse>(0);
        }

        Item[] stories;

        var bestStoriesIdsToProcess = bestStoriesIds.Take(amountOfItems).ToList();
        var changedItems = await hackerNewsApi.GetChangedItemsAsync(cancellationToken);

        if (changedItems.Items is { Count: > 0 })
        {
            stories = await CheckCacheAsync(bestStoriesIdsToProcess, changedItems.Items, cancellationToken);
        }
        else
        {
            stories = await CheckCacheAsync(bestStoriesIdsToProcess, cancellationToken: cancellationToken);
        }

        if (!AreAllItemStories(stories))
        {
            throw new InvalidOperationException("Not all items are stories");
        }

        var outputStories = mapper.Map<List<GetBestStories.BestStoriesResponse>>(stories);

        return outputStories.OrderByDescending(s => s.Score).ToList();
    }

    private async Task<Item[]> CheckCacheAsync(
        IReadOnlyList<int> storiesIds,
        IReadOnlyList<int>? changedItemsIds = null,
        CancellationToken cancellationToken = default)
    {
        var stories = new ConcurrentBag<Item>();
        var tasks = new List<Task>();

        if (changedItemsIds is null)
        {
            tasks.AddRange(storiesIds.Select(async storyId =>
            {
                var story = await cacheService.GetStoryFromCacheOrFromApiAsync(storyId, cancellationToken);
                stories.Add(story);
            }));

            await Task.WhenAll(tasks.ToArray());

            return stories.ToArray();
        }

        var updatedStoriesIds = storiesIds.Intersect(changedItemsIds).ToList();

        if (updatedStoriesIds is { Count: > 0 })
        {
            tasks.AddRange(updatedStoriesIds.Select(async storyId =>
            {
                var story = await cacheService.GetStoryFromCacheOrFromApiAsync(storyId, cancellationToken, true);
                stories.Add(story);
            }));

            tasks.AddRange(storiesIds.Except(updatedStoriesIds).Select(async storyId =>
            {
                var story = await cacheService.GetStoryFromCacheOrFromApiAsync(storyId, cancellationToken);
                stories.Add(story);
            }));
        }
        else
        {
            tasks.AddRange(storiesIds.Select(async storyId =>
            {
                var story = await cacheService.GetStoryFromCacheOrFromApiAsync(storyId, cancellationToken);
                stories.Add(story);
            }));
        }

        await Task.WhenAll(tasks.ToArray());

        return stories.ToArray();
    }

    private static bool AreAllItemStories(IEnumerable<Item> stories)
    {
        return stories.All(s => s.Type.Equals(ItemType.Story.ToString(), StringComparison.CurrentCultureIgnoreCase));
    }
}