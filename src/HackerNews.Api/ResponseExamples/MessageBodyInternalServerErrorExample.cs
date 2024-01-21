using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HackerNews.Api.ResponseExamples;

/// <summary>
/// Message body for internal server error
/// </summary>
public class MessageBodyInternalServerErrorExample : IExamplesProvider<ProblemDetails>
{
    /// <summary>
    /// Get examples
    /// </summary>
    /// <returns>Problem details</returns>
    public ProblemDetails GetExamples()
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server error"
        };
    }
}