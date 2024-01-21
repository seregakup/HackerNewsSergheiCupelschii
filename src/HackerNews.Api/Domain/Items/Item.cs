namespace Api.Domain.Items;

/// <summary>
/// Model fot the item from the Hacker News API
/// </summary>
public class Item
{
    /// <summary>
    /// The username of the item's author.
    /// </summary>
    /// <example>dhouston</example>
    public required string By { get; init; }
        
    /// <summary>
    /// In the case of stories or polls, the total comment count.
    /// </summary>
    /// <example>16</example>
    public int Descendants { get; init; }
        
    /// <summary>
    /// The item's unique id
    /// </summary>
    /// <example>8863</example>
    public int Id { get; init; }

    /// <summary>
    /// The story's score, or the votes for a pollopt.
    /// </summary>
    /// <example>111</example>
    public int Score { get; init; }
        
    /// <summary>
    /// Creation date of the item, in Unix Time.
    /// </summary>
    /// <example>1314211127</example>
    public int Time { get; init; }
        
    /// <summary>
    /// The title of the story, poll or job. HTML.
    /// </summary>
    /// <example> Some text</example>
    public required string Title { get; init; }
        
    /// <summary>
    /// The type of item. One of "job", "story", "comment", "poll", or "pollopt".
    /// </summary>
    /// <example>story</example>
    public required string Type { get; init; }

    /// <summary>
    /// Url
    /// </summary>
    /// <example>https://www.google.com</example>
    public string? Url { get; set; }
}