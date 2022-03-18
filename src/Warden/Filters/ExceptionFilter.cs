using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Warden.Exceptions;

namespace Warden.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Type = context.Exception.GetType().Name,
            Detail = context.Exception.Message,
            Status = context.Exception switch
            {
                ScheduleEntryNotFoundException
                    or UserNotFoundException => 404,
                ScheduleEntryAlreadyTakenException => 418,
                _ => 500
            }
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = details.Status
        };

        context.ExceptionHandled = true;
    }
}
