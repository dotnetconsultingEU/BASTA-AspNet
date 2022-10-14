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

namespace dotnetconsulting.UserManagement.Infrastructure.Exceptions;

public class UserErrorDetails
{
    public int Id { get; set; }

    public string Message { get; set; }

    public object Details { get; set; }

    public static UserErrorDetails Create(UserException ex)
    {
        return new UserErrorDetails
        {
            Id = ex.EventId.Id,
            Message = ex.Message,
            Details = ex.Details
        };
    }

    public override string ToString() => $"{nameof(UserErrorDetails)}: {Id}";
}