// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using dotnetconsulting.UserManagement.Logic.EntityFramework;
using dotnetconsulting.UserManagement.Logic;
using dotnetconsulting.UserManagement.Infrastructure.Settings;
using dotnetconsulting.UserManagement.Infrastructure.Interfaces;
using dotnetconsulting.UserManagement.WebAPI.Code.ExceptionHandler;
using dotnetconsulting.UserManagement.WebAPI.Code.ApiKey;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
});

builder.Host.UseWindowsService(o =>
{
    o.ServiceName = "dotnetconsultingUserManagement";
});

#region Configuration
#if RELEASE
builder.Configuration.AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true, reloadOnChange: true);
#else
string? customEnvironment = builder.Configuration.GetValue<string>("DNC_ENVIRONMENT");

if (customEnvironment is not null)
    builder.Configuration.AddJsonFile($"appsettings.{customEnvironment}.json", optional: true, reloadOnChange: true);
#endif
#endregion

#region Kestrel
int tcpPort = builder.Configuration.GetValue<int>("Kestrel:TcpPort", 5000);
builder.WebHost.ConfigureKestrel(o =>
{
    o.ListenAnyIP(tcpPort);
});
#endregion

#region API key
IConfigurationSection apiKeyConfigurationSection = builder.Configuration.GetSection("ApiKey");
builder.Services.Configure<ApiKeySettings>(apiKeyConfigurationSection);
ApiKeySettings apiKeySettings = new();
apiKeyConfigurationSection.Bind(apiKeySettings);
#endregion

#region Controllers
// Set up controllers
builder.Services.AddControllers(o =>
{
    if (apiKeySettings.ProtectWithApiKey)
        o.Filters.Add(new ApiKeyFilter(apiKeySettings));

    o.Filters.Add(new UserExceptionHandler());
    o.Filters.Add(new ArgumentOutOfRangeExceptionHandler());
    o.Filters.Add(new TaskCanceledExceptionHandler());
}).AddNewtonsoftJson(o =>
{
    o.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Unspecified;
    o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
});
#endregion

#region Swagger
// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "dotnetconsulting.UserMangement.WebApi",
        Description = $"dotnetconsulting.UserMangement.WebApi<br>API key <b>{apiKeySettings.Key ?? "-"}</b>",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Thorsten Kansy",
            Email = "tkansy@dotnetconsulting.eu",
            Url = new Uri("https://dotnetconsulting.eu"),
        },
        License = new OpenApiLicense
        {
            Name = "Property of dotnetconsulting.eu",
            Url = new Uri("https://www.dotnetconsulting.de/"),
        }
    });

    if (apiKeySettings.ProtectWithApiKey)
        // Pass Swagger API Key
        c.OperationFilter<SwaggerApiKeyFilter>();

    // Pass JWT token if required
    //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //{
    //    In = ParameterLocation.Header,
    //    Description = "Bearer + Token",
    //    Name = "Authorization",
    //    Type = SecuritySchemeType.ApiKey
    //});

    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //            new OpenApiSecurityScheme
    //            {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //            },
    //            Array.Empty<string>()
    //    }
    //});

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null);

    c.EnableAnnotations();
});
#endregion

#region Nlog
string nlogConfigFileName = Path.Combine(AppContext.BaseDirectory, "nlog.config");
if (File.Exists(nlogConfigFileName))
{
    builder.Services.AddLogging(b =>
    {
        b.AddNLog(nlogConfigFileName);
        builder.WebHost.UseNLog();
        builder.Host.UseNLog();
    });
}
#endregion

#region EF Core
builder.Services.AddDbContextPool<UserManagementContext>(o =>
{
    string conString = builder.Configuration.GetConnectionString("Main");
    o.UseSqlServer(conString);
});
#endregion

#region Environment
var environmentSettings = builder.Configuration.GetSection("EnviromentSettings");
builder.Services.Configure<EnvironmentSettings>(environmentSettings);
#endregion

builder.Services.AddScoped<ILogic, Logic>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();