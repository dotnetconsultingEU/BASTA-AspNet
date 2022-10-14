// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.AspNetCore.Mvc.Filters;

namespace dotnetconsulting.UserManagement.WebAPI.Code.ExceptionHandler;

public sealed class TaskCanceledExceptionHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is TaskCanceledException ex)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger<TaskCanceledExceptionHandler>>();

            logger?.LogError(ex, "TaskCanceledException");

            // Nginx' non-standard code 499 or 444.
            context.HttpContext.Response.StatusCode = 499;
            context.ExceptionHandled = true;
        }

        base.OnException(context);
    }
}