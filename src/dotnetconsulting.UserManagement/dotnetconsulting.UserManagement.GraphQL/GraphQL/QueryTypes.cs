// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.UserManagement.Infrastructure.DTOs;
using dotnetconsulting.UserManagement.Infrastructure.Interfaces;
using dotnetconsulting.UserManagement.Logic.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace dotnetconsulting.UserManagement.GraphQL.GraphQL;

public class QueryTypes
{
    public Task<IEnumerable<UserDto>> AllUsers([Service] ILogic logic, CancellationToken cancellationToken)
    {
        return logic.FetchAllUsersAsync(cancellationToken);
    }

    public Task<IEnumerable<PermissionDto>> AllPermissions([Service] ILogic logic, CancellationToken cancellationToken)
    {
        return logic.FetchAvailablePermissionsAsync(cancellationToken);
    }

    public Task<IEnumerable<PlantDto>> AllPlants([Service] ILogic logic, CancellationToken cancellationToken)
    {
        return logic.FetchAvailablePlantsAsync(cancellationToken);
    }

    public async Task<UserDto> GetSingleUser(string uNumber, [Service] ILogic logic, CancellationToken cancellationToken)
    {
        var rawResult = await logic.FetchSingleUserAsync(uNumber, cancellationToken);

        return rawResult;
    }

    #region Queryables
    [UseDbContext(typeof(UserManagementContext))]
    [UsePaging]
    // [UseOffsetPaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User> GetUsersDirect([ScopedService] UserManagementContext context)
    {
        return context.Users.AsNoTracking();
    }

    [UseDbContext(typeof(UserManagementContext))]
    [UsePaging]
    // [UseOffsetPaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Role> GetRolesDirect([ScopedService] UserManagementContext context)
    {
        return context.Roles.AsNoTracking();
    }
    #endregion
}