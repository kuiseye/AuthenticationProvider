using AuthenticationProvider.Application.Errors;
using AuthenticationProvider.Configuration.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AuthenticationProvider.AppInterface.Controllers.Common;

public class BaseController : ControllerBase
{
    protected ObjectResult MapErrors(AppError error)
    {

        var statusCode =  error switch
        {
            NotFoundError _ => 404,
            ValidationError _ => 400,
            InvalidInputError _ => 400,
            UnauthorizedError _ => 401,
            AppError _ => 500
        };
        var problemDetails = new ProblemDetails()
        {
            Type = "about:blank",
            Title = error.Title,
            Detail = error.Message,
            Instance = HttpContext.Request.Path,
            Status = statusCode
        };
        problemDetails.Extensions["code"] = error.Code;
        
        if(error is ValidationError validationError)
            problemDetails.Extensions["validationErrorEntries"] = validationError.ValidationErrorEntries;

        return StatusCode(statusCode, problemDetails);
    }
}