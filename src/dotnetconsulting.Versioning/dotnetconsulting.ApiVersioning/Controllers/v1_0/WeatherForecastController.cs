// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.ApiVersioning.Models.V1_0;
using Microsoft.AspNetCore.Mvc;

namespace dotnetconsulting.ApiVersioning.Controllers.V1_0;

/// <summary>
/// Version 1
/// </summary>
[ApiController, ApiVersion("1.0", Deprecated = true)]
// [ApiVersionNeutral]
[Route("api/v{version:apiVersion}/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "FreezingV1", "BracingV1", "ChillyV1", "CoolV1", "MildV1", "WarmV1", "BalmyV1", "HotV1", "SwelteringV1", "ScorchingV1"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    /// <summary>
    /// Ctor V1
    /// </summary>
    /// <param name="logger"></param>
    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Weather forecast V1.0
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetWeatherForecast"), MapToApiVersion("1.0")]
    public IEnumerable<WeatherForecast> Get()
    {
        // https://localhost:7254/api/v1.0/WeatherforeCast

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}