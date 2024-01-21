using HackerNews.Api.Domain.Items;
using HackerNews.Api.Infrastructure.ExternalApi.Models;
using Refit;

namespace HackerNews.Api.Infrastructure.ExternalApi;

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
    Task<int[]> GetBestStoriesIdsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Get a item by its id from the Hacker News API
    /// </summary>
    /// <param name="id">2922097</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A detailed information about the item</returns> 
    [Get("/v0/item/{id}.json")]
    Task<Item> GetItemByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Get a model with ids for changed items from the Hacker News API
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A model with ids for changed items</returns>
    [Get("/v0/updates.json")] 
    Task<UpdatesResponse> GetChangedItemsAsync(CancellationToken cancellationToken);
}