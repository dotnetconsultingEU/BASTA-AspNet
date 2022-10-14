// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Xunit.Abstractions;

namespace dotnetconsulting.UserManagement.xUnitTests;

[UsesVerify]
[Trait("Category", "Demo")]
public class LogicQueriesTest : IDisposable
{
    const string GRAPHQLENDPOINT = "https://localhost:7076/graphql";

    private readonly GraphQLHttpClient graphQLClient;

    private readonly ITestOutputHelper _textOutputhelper;

    public LogicQueriesTest(ITestOutputHelper textOutputhelper)
    {
        _textOutputhelper = textOutputhelper;
        graphQLClient = new GraphQLHttpClient(GRAPHQLENDPOINT, new NewtonsoftJsonSerializer());
    }

    public void Dispose()
    {
        graphQLClient?.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task DirectUserPaging()
    {
        // Arrange 
        var request = new GraphQLRequest
        {
            Query = EmbeddedResourceHelper.QueryText("LogicQueries.DirectUserPaging.ql")
        };

        // Act 
        GraphQLResponse<object> result = await graphQLClient.SendQueryAsync<object>(request);

        // Assert
        await Verify(result.Data).UseDirectory("snapshots");
    }

    [Fact]
    public async Task FetchSingleUser()
    {
        // Arrange 
        var request = new GraphQLRequest
        {
            Query = EmbeddedResourceHelper.QueryText("LogicQueries.FetchSingleUser.ql")
        };

        // Act 
        GraphQLResponse<object> result = await graphQLClient.SendQueryAsync<object>(request);

        // Assert
        await Verify(result.Data).UseDirectory("snapshots");
    }
}