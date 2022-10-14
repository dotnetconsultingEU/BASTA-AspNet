// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.UserManagement.GraphQL.GraphQL;
using dotnetconsulting.UserManagement.Infrastructure.Interfaces;
using dotnetconsulting.UserManagement.Infrastructure.Settings;
using dotnetconsulting.UserManagement.Logic;
using dotnetconsulting.UserManagement.Logic.EntityFramework;
using GraphQL.Server.Ui.Voyager;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region Nlog
string nlogConfigFileName = System.IO.Path.Combine(AppContext.BaseDirectory, "nlog.config");
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
builder.Services.AddPooledDbContextFactory<UserManagementContext>(o =>
{
    string conString = builder.Configuration.GetConnectionString("Main");
    o.UseSqlServer(conString);
});

builder.Services.AddDbContextPool<UserManagementContext>(o =>
{
    string conString = builder.Configuration.GetConnectionString("Main");
    o.UseSqlServer(conString);
});
#endregion

#region HotChocolate
builder.Services.AddGraphQLServer()
                .AddProjections()
                .AddSorting()
                .AddFiltering()
                .AddQueryType<QueryTypes>()
                .AddMutationType<Mutations>();
#endregion

#region Environment
var environmentSettings = builder.Configuration.GetSection("EnviromentSettings");
builder.Services.Configure<EnvironmentSettings>(environmentSettings);
#endregion

#region AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
#endregion

builder.Services.AddScoped<ILogic, Logic>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.UseGraphQLVoyager("/graphql-voyager", new VoyagerOptions()
{
    GraphQLEndPoint = "/graphql"
});

app.Run();