// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.Extensions.Logging;

namespace dotnetconsulting.UserManagement.Infrastructure.Exceptions;

public class UserException : Exception
{
    public readonly EventId EventId;
    public readonly object Details;

    public UserException(EventId EventId, object Details) : base($"EventId={EventId.Id} ({EventId.Name}) was thrown")
    {
        this.EventId = EventId;
        this.Details = Details!;
    }

    public UserException(EventId EventId) : this(EventId, null!)
    {
    }
}