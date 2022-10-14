// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using System.Reflection;

namespace dotnetconsulting.UserManagement.xUnitTests
{
    public static class EmbeddedResourceHelper
    {
        public static string QueryText(string ResourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            // dotnetconsulting.UserManagement.GraphQLClient
            using Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{ResourceName}")!;
            using StreamReader reader = new(stream);
            return reader.ReadToEnd();
        }
    }
}