using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.BestNews;

/// <summary>
/// GetBestNews feature
/// </summary>
public static class GetBestNews
{
    /// <summary>
    /// Query for getting items
    /// </summary>
    public class Query : IRequest<IReadOnlyList<BestNewsResponse>>
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
    public class BestNewsResponse
    {
    }

    internal sealed class Handler : IRequestHandler<Query, IReadOnlyList<BestNewsResponse>>
    {
        public async Task<IReadOnlyList<BestNewsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
                var result = await sender.Send(new GetBestNews.Query { AmountOfItems = amountOfItems });
                return result;
            })
            .Produces<IReadOnlyList<GetBestNews.BestNewsResponse>>()
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Returns the best news from HackerNews API")
            .WithOpenApi();
    }
}