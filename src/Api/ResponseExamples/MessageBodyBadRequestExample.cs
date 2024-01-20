using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Api.ResponseExamples;

/// <summary>
/// Message body for bad request
/// </summary>
public class MessageBodyBadRequestExample : IExamplesProvider<ProblemDetails>
{
    /// <summary>
    /// Get examples
    /// </summary>
    /// <returns>Problem details</returns>
    public ProblemDetails GetExamples()
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Bad Request",
            Detail = "Amount of items must be between 1 and 500"
        };
    }
}