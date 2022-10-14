// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.UserManagement.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace dotnetconsulting.UserManagement.WebAPI.Code.ApiKey;

/// <summary>
/// Filter to check api key.
/// </summary>
public class ApiKeyFilter : IAuthorizationFilter
{
    /// <summary>
    /// Name of header.
    /// </summary>
    public const string APIKEYNAME = "x-api-key";

    /// <summary>
    /// The key od <c>null</c>.
    /// </summary>
    private readonly string _apiKey;

    public ApiKeyFilter(ApiKeySettings Settings)
    {
        _apiKey = Settings?.Key!;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        bool skipApiKeyCheck = false;

        // Currently we only skip the check if the Info-Controller is invoked.
        if (context.ActionDescriptor is ControllerActionDescriptor cad)
        {
            // Skip for Info-Controller
            skipApiKeyCheck = cad.ControllerTypeInfo.AsType() == typeof(InfoController);
        }

        if (skipApiKeyCheck)
            return;

        // Verify API key
        string apiKey = context.HttpContext.Request.Headers[APIKEYNAME].ToString();

        if (string.Compare(_apiKey, apiKey) != 0)
            context.Result = new UnauthorizedResult();
    }
}