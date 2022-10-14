// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection;

namespace dotnetconsulting.UserManagement.WebAPI.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class InfoController : ControllerBase
{
    private readonly IActionDescriptorCollectionProvider _provider;

    public InfoController(IActionDescriptorCollectionProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Returns a list of all files (currently assemblies).
    /// </summary>
    /// <returns></returns>
    [HttpGet("Files")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(IList<string>))]
    public ActionResult<IList<string>> GetAssemblies()
    {
        // Return a list of loaded assemblies
        AppDomain currentDomain = AppDomain.CurrentDomain;

        //Make an array for the list of assemblies.
        Assembly[] assemblies = currentDomain.GetAssemblies();

        IList<string> result = new List<string>();

        // Add infos as needed

        //List the assemblies in the current application domain.
        foreach (Assembly asm in assemblies.Where(w => w.FullName!.StartsWith("dotnetconsulting")).OrderBy(o => o.ToString()))
            result.Add(asm.ToString());
        foreach (Assembly asm in assemblies.Where(w => !w.FullName!.StartsWith("dotnetconsulting")).OrderBy(o => o.ToString()))
            result.Add(asm.ToString());

        // Finished
        return Ok(result);
    }

    /// <summary>
    /// Returns a list of all routes.
    /// </summary>
    /// <returns></returns>
    [HttpGet("Routes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(IList<string>))]
    public ActionResult<IList<string>> GetRoutes()
    {
        var routes = _provider.ActionDescriptors.Items.Select(x => new
        {
            Action = x.RouteValues["Action"],
            Controller = x.RouteValues["Controller"],
            x.AttributeRouteInfo!.Name,
            x.AttributeRouteInfo.Template
        }).ToList();

        return Ok(routes);
    }
}