// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.UserManagement.Infrastructure.DTOs;

namespace dotnetconsulting.UserManagement.Infrastructure.Interfaces;

public interface ILogic
{
    /// <summary>
    /// (1) Add/ Update user.
    /// </summary>
    /// <param name="User">The user to be created.</param>
    /// <param name="cancellationToken">Async cancellation token.</param>
    /// <returns></returns>
    Task<UserUpdateMode> AddOrUpdateUserAsync(FullUserDto User, CancellationToken cancellationToken);

    /// <summary>
    /// (2) Fetches all available permissions existing in the system.
    /// </summary>
    /// <param name="cancellationToken">Async cancellation token.</param>
    /// <returns></returns>
    Task<IEnumerable<PermissionDto>> FetchAvailablePermissionsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// (3) Fetche all users with permissions.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<UserDto>> FetchAllUsersAsync(CancellationToken cancellationToken);

    /// <summary>
    /// (4) Fetch one user.
    /// </summary>
    /// <param name="UNumber">User Id.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UserDto> FetchSingleUserAsync(string UNumber, CancellationToken cancellationToken);

    /// <summary>
    /// (5) Deactivate user.
    /// </summary>
    /// <param name="UNumber">User Id.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeactivateUserAsync(string UNumber, CancellationToken cancellationToken);

    /// <summary>
    /// (6) Sync the permissions for an existing user.
    /// </summary>
    /// <param name="SyncPermissions">The permissions to be added/ removed.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SyncUserPermissionsAsync(SyncPermissionsDto SyncPermissions, CancellationToken cancellationToken);

    /// <summary>
    /// (7) Fetches all available plants existing in the system.
    /// </summary>
    /// <param name="cancellationToken">Async cancellation token.</param>
    /// <returns></returns>
    Task<IEnumerable<PlantDto>> FetchAvailablePlantsAsync(CancellationToken cancellationToken);
}