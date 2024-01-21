namespace HackerNews.Api.Features.BestStories;

/// <summary>
/// Interface for the Hacker News Service
/// </summary>
public interface IHackerNewsService
{
    /// <summary>
    /// Returns the best news sorted by score
    /// </summary>
    /// <param name="amountOfItems">Amount of items to return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A collection of sorted best news</returns>
    Task<IReadOnlyList<GetBestStories.BestStoriesResponse>> GetSortedByScoreAmountOfBestNewsAsync(int amountOfItems, CancellationToken cancellationToken);
}