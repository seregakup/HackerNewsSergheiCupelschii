using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.Items;

/// <summary>
/// GetItems feature
/// </summary>
public static class GetItems
{
    /// <summary>
    /// Query for getting items
    /// </summary>
    public class Query : IRequest<IEnumerable<ItemsResponse>>
    {
        /// <summary>
        /// The amount of items to return
        /// </summary>
        /// <example>25</example>
        public int AmountOfItems { get; set; }
    }

    /// <summary>
    /// Response for getting items
    /// </summary>
    public class ItemsResponse
    {
        
    }

    internal sealed class Handler : IRequestHandler<Query, IEnumerable<ItemsResponse>>
    {
        public async Task<IEnumerable<ItemsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

/// <summary>
/// Method to map the endpoint for getting items
/// </summary>
public class GetItemsEndpoint : ICarterModule
{
    /// <summary>
    /// Add the routes for getting items
    /// </summary>
    /// <param name="app"></param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/items", [SwaggerOperation(
                Summary = "returns news from HackerNews API")]
            [SwaggerResponse(200, "success")]
            [SwaggerResponse(500, "failure")] async (ISender sender, [FromQuery]int amountOfItems = 25)
            =>
        {
            var result = await sender.Send(new GetItems.Query { AmountOfItems = amountOfItems });
            return result;
        });
    }
}