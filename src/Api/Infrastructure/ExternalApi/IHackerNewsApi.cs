using Api.Domain.Items;
using Refit;

namespace Api.Infrastructure.ExternalApi;

/// <summary>
/// Interface for the Hacker News API
/// </summary>
public interface IHackerNewsApi
{
    /// <summary>
    /// Get the best stories' ids from the Hacker News API
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A collection of the best stories' ids</returns>
    [Get("/v0/beststories.json")]
    Task<int[]> GetBestStoriesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Get a item by its id from the Hacker News API
    /// </summary>
    /// <param name="id">2922097</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A detailed information about the item</returns> 
    [Get("/v0/item/{id}.json")]
    Task<Item> GetItemByIdAsync(int id, CancellationToken cancellationToken);
}