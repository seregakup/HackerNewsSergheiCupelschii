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
    /// <returns>A collection of the best stories' ids</returns>
    [Get("/v0/beststories.json")]
    Task<int[]> GetBestStories();

    /// <summary>
    /// Get a story by its id from the Hacker News API
    /// </summary>
    /// <param name="id">2922097</param>
    /// <returns>A detailed information about the item</returns> 
    [Get("/v0/item/{id}.json")]
    Task<Item> GetItemById(int id);
}