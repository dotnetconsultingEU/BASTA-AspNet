// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschr�nkungen verwendet oder ver�ndert werden.
// Jedoch wird keine Garantie �bernommen, dass eine Funktionsf�higkeit mit aktuellen und 
// zuk�nftigen API-Versionen besteht. Der Autor �bernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgef�hrt wird.
// F�r Anregungen und Fragen stehe ich jedoch gerne zur Verf�gung.

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