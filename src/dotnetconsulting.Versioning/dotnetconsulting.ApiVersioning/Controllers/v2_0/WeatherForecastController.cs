// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.ApiVersioning.Models.V2_0;
using Microsoft.AspNetCore.Mvc;

namespace dotnetconsulting.ApiVersioning.Controllers.V2_0;

/// <summary>
/// Version 2
/// </summary>
[ApiController, ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "FreezingV2", "BracingV2", "ChillyV2", "CoolV2", "MildV2", "WarmV2", "BalmyV2", "HotV2", "SwelteringV2", "ScorchingV2"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    /// <summary>
    /// Ctor V2
    /// </summary>
    /// <param name="logger"></param>
    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Weather forecast V2.0
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetWeatherForecast"), MapToApiVersion("2.0")]
    public IEnumerable<WeatherForecast> Get()
    {
        // https://localhost:7254/api/v2.0/WeatherforeCast
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}