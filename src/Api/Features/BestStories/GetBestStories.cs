using Api.Services;
using AutoMapper;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.BestStories;

/// <summary>
/// GetBestStories feature
/// </summary>
public static class GetBestStories
{
    /// <summary>
    /// Query for getting items
    /// </summary>
    public class Query : IRequest<IReadOnlyList<BestStoriesResponse>>
    {
        /// <summary>
        /// The amount of items to return
        /// </summary>
        /// <example>25</example>
        public int AmountOfItems { get; set; }
    }

    /// <summary>
    /// Response for getting items from HackerNews API
    /// </summary>
    public class BestStoriesResponse
    {
        /// <summary>
        /// The username of the item's author.
        /// </summary>
        /// <example>dhouston</example>
        public required string By { get; set; }
        
        /// <summary>
        /// In the case of stories or polls, the total comment count.
        /// </summary>
        /// <example>16</example>
        public int Descendants { get; set; }
        
        /// <summary>
        /// The item's unique id
        /// </summary>
        /// <example>8863</example>
        public int Id { get; set; }
        
        /// <summary>
        /// The ids of the item's comments, in ranked display order.
        /// </summary>
        /// <example>[ 2922097, 2922429, 2924562 ]</example>
        public required int[] Kids { get; set; }
       
        /// <summary>
        /// The story's score, or the votes for a pollopt.
        /// </summary>
        /// <example>111</example>
        public int Score { get; set; }
        
        /// <summary>
        /// The comment, story or poll text. HTML.
        /// </summary>
        /// <example> Some text</example>
        public required string Text { get; set; }
        
        /// <summary>
        /// Creation date of the item, in Unix Time.
        /// </summary>
        /// <example>1314211127</example>
        public int Time { get; set; }
        
        /// <summary>
        /// The title of the story, poll or job. HTML.
        /// </summary>
        /// <example> Some text</example>
        public required string Title { get; set; }
        
        /// <summary>
        /// The type of item. One of "job", "story", "comment", "poll", or "pollopt".
        /// </summary>
        /// <example>story</example>
        public required string Type { get; set; }
    }


    internal sealed class Handler(IHackerNewsService hackerNewsService, IMapper mapper) : IRequestHandler<Query, IReadOnlyList<BestStoriesResponse>>
    {
        public async Task<IReadOnlyList<BestStoriesResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var bestStories = await hackerNewsService.GetSortedBestNewsByScoreAsync(cancellationToken);

            if (bestStories.Count == 0)
            {
                return new List<BestStoriesResponse>(0);
            }

            return bestStories.Take(request.AmountOfItems).ToList();
        }
    }
}

/// <summary>
/// Method to map the endpoint for getting best news
/// </summary>
public class GetBestNewsEndpoint : ICarterModule
{
    /// <summary>
    /// Add the routes for getting items
    /// </summary>
    /// <param name="app"></param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/best-news", async (ISender sender, [FromQuery] int amountOfItems = 25)
                =>
            {
                var result = await sender.Send(new GetBestStories.Query { AmountOfItems = amountOfItems });
                return result;
            })
            .Produces<IReadOnlyList<GetBestStories.BestStoriesResponse>>()
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Returns the best news from HackerNews API")
            .WithOpenApi();
    }
}