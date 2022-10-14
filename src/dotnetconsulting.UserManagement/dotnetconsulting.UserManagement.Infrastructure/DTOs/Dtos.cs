// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

namespace dotnetconsulting.UserManagement.Infrastructure.DTOs;

public record PermissionDto(string Permission);

public record PlantDto(string Code);

public record UserDto(string UNumber, string? Username, string? Firstname, string? Lastname,
    string? Email, string? Culture, string? Status, IEnumerable<PlantDto>? Plants, IEnumerable<PermissionDto>? Permissions);

public record FullUserDto(UserDto? UserAccount, IEnumerable<PlantDto>? RequestedPlants, IEnumerable<PermissionDto>? RequestedPermissions);

public record SyncPermissionsDto(string UNumber, IEnumerable<PermissionDto>? PermissionsToAdd, IEnumerable<PermissionDto>? PermissionsToDrop);