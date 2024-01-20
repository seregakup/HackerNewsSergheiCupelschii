namespace Api.Infrastructure.ExternalApi.Models;

/// <summary>
/// Response for updates
/// </summary>
public class UpdatesResponse
{
    /// <summary>
    /// Collection of ids for changed items
    /// </summary>
    public List<int>? Items { get; set; }
}