// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.UserManagement.Infrastructure.DTOs;
using dotnetconsulting.UserManagement.Infrastructure.Exceptions;
using dotnetconsulting.UserManagement.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace dotnetconsulting.UserManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserManagementController : ControllerBase
{
    private readonly ILogic _logic;
    private readonly ILogger<UserManagementController> _logger;

    public UserManagementController(ILogic logic,
                                  ILogger<UserManagementController> logger)
    {
        _logic = logic;
        _logger = logger;

        _logger.LogTrace("Controller created");
    }

    /// <summary>
    /// (1) Add/ Update user.
    /// </summary>
    /// <param name="User">The user to be created.</param>
    /// <param name="cancellationToken">Async cancellation token.</param>
    /// <returns></returns>
    [HttpPut("User")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status200OK, "User was refreshed.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [SwaggerResponse(StatusCodes.Status201Created, "User was created.")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Specific error. See details.", typeof(UserErrorDetails))]
    public async Task<ActionResult> AddOrUpdateUser(FullUserDto User, CancellationToken cancellationToken)
    {
        var result = await _logic.AddOrUpdateUserAsync(User, cancellationToken);

        switch (result)
        {
            case UserUpdateMode.UserWasCreated:
                var uri = new Uri($"/api/user/{User!.UserAccount!.UNumber}", UriKind.Relative);
                return Created(uri, null);
            case UserUpdateMode.UserWasUpdated:
                return Ok();
            default:
                throw new NotImplementedException();
        }
    }

    /// <summary>
    /// (2) Fetch all available permissions existing in the system.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("AllPermissions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(IEnumerable<PermissionDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Specific error. See details.", typeof(UserErrorDetails))]
    public async Task<ActionResult<IEnumerable<PermissionDto>>> FetchAvailablePermissions(CancellationToken cancellationToken)
    {
        var result = await _logic.FetchAvailablePermissionsAsync(cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// (3) Fetch all users with permissions.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("AllUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(IEnumerable<UserDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Specific error. See details.", typeof(UserErrorDetails))]
    public async Task<ActionResult<IEnumerable<UserDto>>> FetchAllUsers(CancellationToken cancellationToken)
    {
        var result = await _logic.FetchAllUsersAsync(cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// (4) Fetch one user.
    /// </summary>
    /// <param name="UNumber">User Id.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("User")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "UNumber is invalid")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Specific error. See details.", typeof(UserErrorDetails))]
    public async Task<ActionResult<UserDto>> FetchSingleUser(string UNumber, CancellationToken cancellationToken)
    {
        var result = await _logic.FetchSingleUserAsync(UNumber, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// (5) Deactivate a user.
    /// </summary>
    /// <param name="UNumber">User Id.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("DeactivateUser")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "UNumber is invalid")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Specific error. See details.", typeof(UserErrorDetails))]
    public async Task<ActionResult> DeactivateUser(string UNumber, CancellationToken cancellationToken)
    {
        await _logic.DeactivateUserAsync(UNumber, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// (6) Sync the permissions for an existing user.
    /// </summary>
    /// <param name="SyncPermissions">The permissions to be added/ removed for the given user.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("Permissions")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "UNumber is invalid")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Specific error. See details.", typeof(UserErrorDetails))]
    public async Task<ActionResult> SyncPermissions(SyncPermissionsDto SyncPermissions, CancellationToken cancellationToken)
    {
        await _logic.SyncUserPermissionsAsync(SyncPermissions, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// (7) Fetch all available plants existing in the system.
    /// </summary>
    /// <param name="cancellationToken">Async cancellation token.</param>
    /// <returns></returns>
    [HttpGet("AllPlants")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(IEnumerable<PlantDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Specific error. See details.", typeof(UserErrorDetails))]
    public async Task<ActionResult<IEnumerable<PlantDto>>> FetchAvailablePlants(CancellationToken cancellationToken)
    {
        var result = await _logic.FetchAvailablePlantsAsync(cancellationToken);

        return Ok(result);
    }
}