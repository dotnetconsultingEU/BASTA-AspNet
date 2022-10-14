// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.UserManagement.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace dotnetconsulting.UserManagement.WebAPI.Code.ExceptionHandler;

public sealed class UserExceptionHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is UserException userEx)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger<UserExceptionHandler>>(); ;

            logger?.LogError("UserException: {userEx.Details}", userEx.Details);

            context.Result = new ObjectResult(UserErrorDetails.Create(userEx)); ;
            context.HttpContext.Response.StatusCode = 400;
            context.ExceptionHandled = true;
        }

        base.OnException(context);
    }
}