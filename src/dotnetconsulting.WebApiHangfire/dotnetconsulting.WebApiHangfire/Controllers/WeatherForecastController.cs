// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.AspNetCore.Mvc;
using Hangfire;

namespace dotnetconsulting.WebApiHangfire.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
        {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public WeatherForecastController(IBackgroundJobClient backgroundJobClient,
                                     ILogger<WeatherForecastController> logger)
    {
        _backgroundJobClient = backgroundJobClient;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get(string Id)
    {
        string jobId = _backgroundJobClient.Schedule(() => DelayedJob(Id), TimeSpan.FromSeconds(120));
        _logger.LogInformation("{DateTime.Now}: Started {Id}", DateTime.Now, Id);

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public void DelayedJob(string Id)
    {
        _logger.LogInformation("{DateTime.Now}: Executed {Id}", DateTime.Now, Id);

        Console.WriteLine($"{DateTime.Now}: {Id}");
    }
}