// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

#pragma warning disable IDE0060 // Remove unused parameter

using dotnetconsulting.UserManagement.Infrastructure.DTOs;
using dotnetconsulting.UserManagement.Infrastructure.Interfaces;
using dotnetconsulting.UserManagement.Logic.EntityFramework;

namespace dotnetconsulting.UserManagement.GraphQL.GraphQL;

public class Mutations
{
    public async Task<DeactivateUserResult> DeactivateUserAsync(string uNumber, [Service] ILogic logic, CancellationToken cancellationToken)
    {
        await logic.DeactivateUserAsync(uNumber, cancellationToken);

        return new DeactivateUserResult("Deactivated");
    }

    public record DeactivateUserResult(string Result);


    public async Task<AddOrUpdateUserResult> AddOrUpdateUser([Service] ILogic logic, FullUserDto user, CancellationToken cancellationToken)
    {
        var result = await logic.AddOrUpdateUserAsync(user, cancellationToken);

        string content = result switch
        {
            UserUpdateMode.UserWasCreated => "UserWasCreated",
            UserUpdateMode.UserWasUpdated => "UserWasUpdated",
            _ => throw new InvalidOperationException()
        };

        return new AddOrUpdateUserResult(content);
    }

    public record AddOrUpdateUserResult(string Result);

    public async Task<UserDto> SyncPermissions([Service] ILogic logic, SyncPermissionsDto permissions, CancellationToken cancellationToken)
    {
        await logic.SyncUserPermissionsAsync(permissions, cancellationToken);

        // Dieser schritte wäre leicht zu optimieren, da keine zwei DB Zugriff nötig sind
        // Hier so um die API mit der WebAPI kompatibel zu halten.
        return await logic.FetchSingleUserAsync(permissions.UNumber, cancellationToken);

        // return new SyncPermissionsResult("Ok");
    }

    public record SyncPermissionsResult(string Result);

    #region Weitere Beispiele
    [UseDbContext(typeof(UserManagementContext))]
    public async Task<JobStartResult> StartJobAsync(int jobId, [ScopedService] UserManagementContext context)
    {
        Console.WriteLine($"input: {jobId}");

        return await Task.FromResult(new JobStartResult(jobId * 2, $"Title: {jobId}"));
    }

    public record JobStartResult(int Id, string Name);
    #endregion
}