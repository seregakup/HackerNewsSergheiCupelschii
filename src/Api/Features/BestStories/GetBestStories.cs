using System.Net;
using System.Text.Json.Serialization;
using Api.Domain.Items;
using Api.ResponseExamples;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

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
        public int AmountOfItems { get; init; }
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
        public required string PostedBy { get; init; }

        /// <summary>
        /// The total comment count.
        /// </summary>
        /// <example>16</example>
        public int CommentCount { get; init; }
      
        /// <summary>
        /// The story's score, or the votes for a pollopt.
        /// </summary>
        /// <example>111</example>
        public int Score { get; init; }

        /// <summary>
        /// Creation date of the item in UTC.
        /// </summary>
        /// <example>2021-10-10T10:10:10+00:00</example>
        public DateTime Time { get; init; }

        /// <summary>
        /// The title of the story, poll or job. HTML.
        /// </summary>
        /// <example>Some text</example>
        public required string Title { get; init; }
        
        /// <summary>
        /// Url
        /// </summary>
        /// <example>https://www.google.com</example>
        public Uri? Uri { get; init; }
    }

    internal sealed class Handler(IHackerNewsService hackerNewsService)
        : IRequestHandler<Query, IReadOnlyList<BestStoriesResponse>>
    {
        public async Task<IReadOnlyList<BestStoriesResponse>> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            var amountOfItems = request.AmountOfItems;
            
            var bestStories = await hackerNewsService.GetSortedByScoreAmountOfBestNewsAsync(amountOfItems, cancellationToken);

            if (bestStories.Count == 0)
            {
                return new List<BestStoriesResponse>(0);
            }

            return bestStories;
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
    /// <param name="app">IEndpointRouteBuilder</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/news-management/best-news",
                [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(MessageBodyOkExample))] 
                [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(MessageBodyBadRequestExample))]
                [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(MessageBodyInternalServerErrorExample))]
                async (ISender sender, [FromQuery] int amountOfItems = 25)
                    =>
                {
                    if (amountOfItems is < 1 or > 500)
                    {
                        throw new ArgumentOutOfRangeException(nameof(amountOfItems),
                            message: "Amount of items must be between 1 and 500");
                    }

                    var result = await sender.Send(new GetBestStories.Query { AmountOfItems = amountOfItems });

                    return result;
                })
            .Produces<IReadOnlyList<GetBestStories.BestStoriesResponse>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Returns certain amounts of the best stories from the HackerNews API")
            .WithOpenApi();
    }
}