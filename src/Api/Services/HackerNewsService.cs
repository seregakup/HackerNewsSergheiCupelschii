using Api.Domain.Items;
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
    /// <param name="amountOfItems">Amount of items to return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A collection of sorted best news</returns>
    public async Task<IReadOnlyList<GetBestStories.BestStoriesResponse>> GetSortedByScoreAmountOfBestNewsAsync(
        int amountOfItems,
        CancellationToken cancellationToken)
    {
        if (amountOfItems is < 1 or > 500)
        {
            throw new ArgumentOutOfRangeException(nameof(amountOfItems), message: "Amount of items must be between 1 and 500");
        }
        
        var bestStoriesIds = await hackerNewsApi.GetBestStoriesIdsAsync(cancellationToken);

        if (bestStoriesIds.Length == 0)
        {
            return new List<GetBestStories.BestStoriesResponse>(0);
        }
        
        var bestStoriesIdsToProcess = bestStoriesIds.Take(amountOfItems).ToList();

        var tasks = bestStoriesIdsToProcess.Select(async storyId =>
        {
            var story = await hackerNewsApi.GetItemByIdAsync(storyId, cancellationToken);
            return story;
        }).ToList();

        var stories = await Task.WhenAll(tasks);

        if (!AreAllItemStories(stories))
        {
            throw new InvalidOperationException("Not all items are stories");
        }

        var outputStories = mapper.Map<List<GetBestStories.BestStoriesResponse>>(stories);

        return outputStories.OrderByDescending(s => s.Score).ToList();
    }

    private static bool AreAllItemStories(IEnumerable<Item> stories)
    {
        return stories.All(s => s.Type.Equals(ItemType.Story.ToString(), StringComparison.CurrentCultureIgnoreCase));
    }
}