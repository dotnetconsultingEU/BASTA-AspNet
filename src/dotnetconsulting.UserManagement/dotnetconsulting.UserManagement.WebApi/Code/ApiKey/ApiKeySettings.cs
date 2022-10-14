// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

#nullable disable

using dotnetconsulting;

namespace dotnetconsulting.UserManagement.WebAPI.Code.ApiKey;

/// <summary>
/// Settings for Api Key.
/// </summary>
public class ApiKeySettings
{
    /// <summary>
    /// The Api key or <c>null</c>. 
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Check if api key protection is avaiable.
    /// </summary>
    public bool ProtectWithApiKey => Key != null;
}