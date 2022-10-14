// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace dotnetconsulting.ApiVersioning;
/// <summary>
/// Configures the Swagger generation options.
/// </summary>
/// <remarks>
/// <para>
/// This allows API versioning to define a Swagger document per API version after the
/// <see cref="IApiVersionDescriptionProvider" /> service has been resolved from the service container.
/// </para>
/// <para>Taken from https://github.com/microsoft/aspnet-api-versioning.</para>
/// </remarks>
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    #region member vars

    private readonly IApiVersionDescriptionProvider _provider;

    #endregion

    #region constructors and destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions" /> class.
    /// </summary>
    /// <param name="provider">
    /// The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger
    /// documents.
    /// </param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    #endregion

    #region explicit interfaces

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        // add a swagger document for each discovered API version
        // note: you might choose to skip or document deprecated API versions differently
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    #endregion

    #region methods

    /// <summary>
    /// Internal implementation for building the Swagger basic config.
    /// </summary>
    /// <param name="description">The description object containing the.</param>
    /// <returns>The generated Open API info.</returns>
    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Sample WeatherForecast API",
            Version = description.ApiVersion.ToString(),
            Description = @"<p>Sample API with versioning including Swagger.</p><p>Quelle: <a href=""https://github.com/microsoft/aspnet-api-versioning"">this repository</a>.</p>",
            Contact = new OpenApiContact
            {
                Name = "dotnetconsulting.eu",
                Email = "inf@dotnetconsulting.eu"
            }
        };
        if (description.IsDeprecated)
        {
            info.Description += @"<p><strong><span style=""color:white;background-color:red"">VERSION IS DEPRECATED</span></strong></p>";
        }
        return info;
    }

    #endregion
}