using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationProvider.Configuration;

internal sealed class GlobalException(IProblemDetailsService problemDetailsService, ILogger<GlobalException> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        Log.InternalError(logger, exception);

        var problemDetails = new ProblemDetails()
        {
            Type = "about:blank",
            Title = "Internal exception",
            Detail = "An internal exception occured. Please inform the developer about it's occurence.",
            Instance = httpContext.Request.Path,
            Status = 500
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception
        });
    }
}