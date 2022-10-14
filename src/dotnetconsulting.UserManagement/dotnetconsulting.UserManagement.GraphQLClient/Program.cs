// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.UserManagement.GraphQLClient;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

// Client erzeugen
const string GRAPHQLENDPOINT = "https://localhost:7076/graphql";

using (var graphQLClient = new GraphQLHttpClient(GRAPHQLENDPOINT, new NewtonsoftJsonSerializer()))
{
    // Request erstellen
    var request = new GraphQLRequest
    {
        Query = EmbeddedResourceHelper.QueryText("LogicQueries.DirectUserPaging.ql")
    };

    // Ausführen
    GraphQLResponse<object> result = await graphQLClient.SendQueryAsync<object>(request);

    Console.WriteLine(result.Data);
}

Console.ReadLine();