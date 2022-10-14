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

/// <summary>
/// All event ids for custom errors the logic can raise.
/// </summary>
public static class UserEventIds
{
    const int IDBASE = 700;

    public static readonly EventId NoProblem = new(IDBASE, "No Problem");
    public static readonly EventId NoPermissionsFound = new(IDBASE + 1, "No permissions found");
    public static readonly EventId NoUNumberSupplied1 = new(IDBASE + 2, "No uNumber supplied");
    public static readonly EventId NoUNumberSupplied2 = new(IDBASE + 3, "No uNumber supplied");
    public static readonly EventId NoUNumberSupplied3 = new(IDBASE + 4, "No uNumber supplied");
    public static readonly EventId InvalidPermission = new(IDBASE + 5, "Invalid permission");
    public static readonly EventId InvalidUserData = new(IDBASE + 6, "Invalid user data");
    public static readonly EventId NoUNumberSupplied4 = new(IDBASE + 7, "No uNumber supplied");
    public static readonly EventId InvalidPlant = new(IDBASE + 8, "Invalid plant");
}