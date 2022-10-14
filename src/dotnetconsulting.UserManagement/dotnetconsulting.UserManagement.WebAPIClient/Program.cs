// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.UserManagement;
using dotnetconsulting.UserManagement.WebAPIClient;

// Racing conditions vermeiden (Nur für Demo)
Task.Delay(3 * 1000).Wait();

while (true)
{
    Console.WriteLine($"== Press any key to call {ConstantValues.BASEURL}{ConstantValues.PATH} ==");
    Console.ReadKey();

    try
    {
        using HttpClient httpClient = new();
        WebAPIHttpClient webApiClient = new(ConstantValues.BASEURL, httpClient);

        // User
        var user = await webApiClient.FetchSingleUserAsync("U000004", ConstantValues.XAPIKEY);

        // All users
        var users = await webApiClient.FetchAllUsersAsync(ConstantValues.XAPIKEY);
    }
    catch (Exception ex)
    {
        Console.WriteLine("== EXCEPTION == ");
        Console.WriteLine(ex);
    }
    Console.WriteLine(new string('=', Console.WindowWidth));
    Console.WriteLine("\n\n");
}