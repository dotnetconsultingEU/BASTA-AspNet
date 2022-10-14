// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using dotnetconsulting.UserManagement.Infrastructure.DTOs;
using dotnetconsulting.UserManagement.Logic.EntityFramework;
using dotnetconsulting.UserManagement.Infrastructure.Settings;
using dotnetconsulting.UserManagement.Infrastructure.Exceptions;
using dotnetconsulting.UserManagement.Infrastructure.Interfaces;

namespace dotnetconsulting.UserManagement.Logic;

public class Logic : ILogic
{
    private readonly UserManagementContext _dbContext;
    private readonly EnvironmentSettings _environmentSettings;
    private readonly ILogger<Logic> _logger;

    public Logic(UserManagementContext dbContext,
                 IOptions<EnvironmentSettings> environmentSettings,
                 ILogger<Logic> logger)
    {
        _dbContext = dbContext;
        _environmentSettings = environmentSettings.Value;
        _logger = logger;
    }

    /// <summary>
    /// (1) Add/ Update user.
    /// </summary>
    /// <param name="FullUser">The user to be created or updated.</param>
    /// <param name="cancellationToken">Async cancellation token.</param>
    /// <returns></returns>
    public async Task<UserUpdateMode> AddOrUpdateUserAsync(FullUserDto FullUser, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"AddOrUpdateUserAsync({FullUser})");

        try
        {
            // Validation
            if (FullUser is null)
                throw new ArgumentNullException(nameof(FullUser));

            // UNumber?
            if (string.IsNullOrWhiteSpace(FullUser!.UserAccount!.UNumber))
                throw new UserException(UserEventIds.NoUNumberSupplied3);
            var uNumer = FullUser!.UserAccount!.UNumber;
            _logger.LogDebug("uNumer: '{uNumer}'", uNumer);

            // User already exists?
            User? user = await _dbContext.Users.Include(i => i.Roles)
                                               .Where(w => w.Name == uNumer)
                                               .FirstOrDefaultAsync(cancellationToken);

            bool userAlreadyExists = user is not null;

            // Validation
            ValidateUser(FullUser.UserAccount!, userAlreadyExists);

            // Create user?
            if (!userAlreadyExists)
            {
                // Create user
                user = new User()
                {
                    // Set properties
                    Email = FullUser.UserAccount.Email,
                    FirstName = FullUser.UserAccount.Firstname,
                    LastName = FullUser.UserAccount.Lastname,
                    Name = FullUser.UserAccount.UNumber,
                    ResourceId = FullUser.UserAccount.Culture ?? _environmentSettings.DefaultCulture
                };

                // Validate requested plants
                await ValidatePassedPlantsAsync(FullUser.RequestedPlants!, cancellationToken);

                // Assign requested plants
                if (FullUser.RequestedPlants?.Any() == true)
                    foreach (var requestedPlant in FullUser.RequestedPlants!.DistinctBy(d => d.Code, StringComparer.InvariantCultureIgnoreCase))
                    {
                        UserPlantCode userPlantCode = new() { Code = requestedPlant.Code };
                        user.UserPlantCodes.Add(userPlantCode);
                    }

                _dbContext.Users.Add(user!);
            }

            // Validate requested permissions
            await ValidatePassedPermissionsAsync(FullUser.RequestedPermissions!, cancellationToken);

            // Sync permissions
            await SyncPermissionsAsync(user!, FullUser.RequestedPermissions!, cancellationToken);

            // Store
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Return result
            if (userAlreadyExists)
                return UserUpdateMode.UserWasUpdated;
            else
                return UserUpdateMode.UserWasCreated;
        }
        catch (UserException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Can't add or update user");
            throw;
        }
    }

    /// <summary>
    /// (2) Fetch all available permissions existing in the system.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<PermissionDto>> FetchAvailablePermissionsAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("FetchAvailableSystemPermissionsAsync()");

        try
        {
            var rawResult = await _dbContext.Roles.AsNoTracking()
                                                  .OrderBy(o => o.Name)
                                                  .ToListAsync(cancellationToken);

            if (rawResult.Any())
                return rawResult.Select(s => new PermissionDto(s.Name));

            throw new UserException(UserEventIds.NoPermissionsFound);
        }
        catch (UserException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Can't fetch system permissions");
            throw;
        }
    }

    /// <summary>
    /// (3) Fetche all users with permissions.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<UserDto>> FetchAllUsersAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("FetchAllUsersAsync()");

        try
        {
            var rawResult = await _dbContext.Users.Include(i => i.Roles)
                                                  .Include(i => i.UserPlantCodes)
                                                  .AsNoTracking()
                                                  .OrderBy(o => o.Name)
                                                  .ToListAsync(cancellationToken);

            var result = rawResult.Select(s => new UserDto(UNumber: s.Name,
                                                           Username: s.Name,
                                                           Firstname: s.FirstName,
                                                           Lastname: s.LastName,
                                                           Email: s.Email,
                                                           Culture: s.ResourceId,
                                                           Plants: s.UserPlantCodes.Select(s => new PlantDto(s.Code)),
                                                           Status: "active",
                                                           Permissions: s.Roles.Select(s => new PermissionDto(s.Name))
                     ));

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Can't fetch system permissions");
            throw;
        }
    }

    /// <summary>
    /// (4) Fetch one user.
    /// </summary>
    /// <param name="UNumber"></param>
    /// <returns></returns>
    public async Task<UserDto> FetchSingleUserAsync(string UNumber, CancellationToken cancellationToken)
    {
        _logger.LogInformation("FetchSingleUserAsync({UNumber})", UNumber);

        try
        {
            if (string.IsNullOrWhiteSpace(UNumber))
                throw new UserException(UserEventIds.NoUNumberSupplied1);

            var rawResult = await _dbContext.Users.Include(i => i.Roles)
                                                  .Include(i => i.UserPlantCodes)
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync(w => w.Name == UNumber, cancellationToken);

            if (rawResult is not null)
            {
                return new UserDto(UNumber: rawResult.Name,
                                   Username: rawResult.Name,
                                   Firstname: rawResult.FirstName,
                                   Lastname: rawResult.LastName,
                                   Email: rawResult.Email,
                                   Culture: rawResult.ResourceId,
                                   Status: "active",
                                   Plants: rawResult.UserPlantCodes.Select(s => new PlantDto(s.Code)),
                                   Permissions: rawResult.Roles.Select(s => new PermissionDto(s.Name))
                    );
            }
            else
                throw new ArgumentOutOfRangeException(nameof(UNumber));
        }
        catch (ArgumentOutOfRangeException)
        {
            throw;
        }
        catch (UserException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Can't fetch available permissions");
            throw;
        }
    }

    /// <summary>
    /// (5) Deactivate a user.
    /// </summary>
    /// <param name="UNumber">User Id.</param>
    /// <returns></returns>
    public async Task DeactivateUserAsync(string UNumber, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DeactivateUser({UNumber})", UNumber);

        try
        {
            if (string.IsNullOrWhiteSpace(UNumber))
                throw new UserException(UserEventIds.NoUNumberSupplied2);

            var user = await _dbContext.Users.FirstOrDefaultAsync(w => w.Name == UNumber, cancellationToken);

            if (user is null)
                throw new ArgumentOutOfRangeException(nameof(UNumber));

            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User '{UNumber}' deactivated", UNumber);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw;
        }
        catch (UserException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Can't deactivate user");
            throw;
        }
    }

    /// <summary>
    /// (6) Sync the permissions for an existing user.
    /// </summary>
    /// <param name="SyncPermissions">The permissions to be added/ removed.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task SyncUserPermissionsAsync(SyncPermissionsDto SyncPermissions, CancellationToken cancellationToken)
    {
        _logger.LogInformation("SyncUserPermissionsAsync({SyncPermissions})", SyncPermissions);

        try
        {
            if (string.IsNullOrWhiteSpace(SyncPermissions.UNumber))
                throw new UserException(UserEventIds.NoUNumberSupplied4);

            var user = await _dbContext.Users.Include(i => i.Roles).FirstOrDefaultAsync(w => w.Name == SyncPermissions.UNumber, cancellationToken);

            if (user is null)
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentOutOfRangeException(nameof(user));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly

                // Loop all permissions to add
            if (SyncPermissions.PermissionsToAdd is not null)
            {
                await ValidatePassedPermissionsAsync(SyncPermissions.PermissionsToAdd, cancellationToken);

                foreach (var permissionToAdd in SyncPermissions.PermissionsToAdd)
                {
                    if (!user.Roles.Any(w => string.Compare(w.Name, permissionToAdd.Permission, true) == 0))
                    {
                        var role = await _dbContext.Roles.FirstAsync(w => w.Name == permissionToAdd.Permission, cancellationToken);

                        user.Roles.Add(role);

                        _logger.LogDebug("Role '{role}' added", permissionToAdd);
                    }
                }
            }

            // Loop all permissions to drop
            if (SyncPermissions.PermissionsToDrop is not null)
            {
                await ValidatePassedPermissionsAsync(SyncPermissions.PermissionsToDrop, cancellationToken);

                foreach (var permissionsToDrop in SyncPermissions.PermissionsToDrop)
                {
                    if (user.Roles.Any(w => string.Compare(w.Name, permissionsToDrop.Permission, true) == 0))
                    {
                        var role = await _dbContext.Roles.FirstAsync(w => w.Name == permissionsToDrop.Permission, cancellationToken);

                        user.Roles.Add(role);

                        _logger.LogDebug("Role '{role}' added", permissionsToDrop);
                    }
                }
            }

            // Store changes
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw;
        }
        catch (UserException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Can't deactivate user");
            throw;
        }
    }

    /// <summary>
    /// (7) Fetches all available plants existing in the system.
    /// </summary>
    /// <param name="cancellationToken">Async cancellation token.</param>
    /// <returns></returns>
    public async Task<IEnumerable<PlantDto>> FetchAvailablePlantsAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("FetchAvailablePlantsAsync()");

        return await Task.FromResult(_environmentSettings.Plants.Distinct().OrderBy(o => o).Select(s => new PlantDto(s)));
    }

    private static void ValidateUser(UserDto user, bool userAlreadyExists)
    {
        // ToDo: More validation?
        if (string.IsNullOrWhiteSpace(user.UNumber))
            throw new UserException(UserEventIds.InvalidUserData, new { user.UNumber });

        if (!userAlreadyExists)
        {
            // First name
            if (string.IsNullOrWhiteSpace(user.Firstname))
                throw new UserException(UserEventIds.InvalidUserData, new { user.Firstname });
            // Last name
            if (string.IsNullOrWhiteSpace(user.Lastname))
                throw new UserException(UserEventIds.InvalidUserData, new { user.Lastname });
            // Email
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new UserException(UserEventIds.InvalidUserData, new { user.Email });
            // Culture
            if (!string.IsNullOrWhiteSpace(user.Culture))
            {
                bool cultureIsValid = CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture => string.Compare(culture.Name, user.Culture, StringComparison.CurrentCultureIgnoreCase) == 0);

                if (!cultureIsValid)
                    throw new UserException(UserEventIds.InvalidUserData, new { user.Culture });
            }
        }
    }

    #region Permissions
    private async Task ValidatePassedPermissionsAsync(IEnumerable<PermissionDto> requestedPermissions, CancellationToken cancellationToken)
    {
        // ToDo: Optimize and remove multiple db acceses

        // Permissions are not case-sensitive
        var invalidPermissions = requestedPermissions!.ExceptBy(await _dbContext.Roles.Select(s => s.Name).ToListAsync(cancellationToken), x => x.Permission, StringComparer.OrdinalIgnoreCase);

        if (invalidPermissions!.Any())
            throw new UserException(UserEventIds.InvalidPermission, new { Invalid = invalidPermissions.Select(s => new { s.Permission }) });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="requestedPermissions"></param>
    /// <param name="cancellationToken"></param>
    private async Task SyncPermissionsAsync(User user, IEnumerable<PermissionDto> requestedPermissions, CancellationToken cancellationToken)
    {
        // Add all requested permissions not in user's permissions
        foreach (PermissionDto? requestedPermission in requestedPermissions)
            if (!user.Roles.Select(s => s.Name).Contains(requestedPermission.Permission, StringComparer.OrdinalIgnoreCase))
            {
                var role = await _dbContext.Roles.SingleAsync(s => s.Name == requestedPermission.Permission, cancellationToken);

                user.Roles.Add(role);
            }

        // Remove user's permissions not requested
        foreach (var obsoletePermission in user.Roles.ExceptBy(requestedPermissions.Select(s => s.Permission), s => s.Name, StringComparer.OrdinalIgnoreCase))
            user.Roles.Remove(obsoletePermission);
    }
    #endregion

    #region Plants
#pragma warning disable IDE0060 // Remove unused parameter
    private async Task ValidatePassedPlantsAsync(IEnumerable<PlantDto> requestedPlants, CancellationToken cancellationToken)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        // Plant codes are not case-sensitive
        var invalidPlants = await Task.FromResult(requestedPlants!.ExceptBy(_environmentSettings.Plants, x => x.Code, StringComparer.OrdinalIgnoreCase));

        if (invalidPlants!.Any())
            throw new UserException(UserEventIds.InvalidPlant, new { Invalid = invalidPlants.Select(s => new { s.Code }) });
    }
    #endregion
}