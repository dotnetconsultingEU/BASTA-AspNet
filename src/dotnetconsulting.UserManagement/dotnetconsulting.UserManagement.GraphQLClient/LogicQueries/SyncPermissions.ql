# Sync permissions
mutation{
  syncPermissions(permissions: {   
    uNumber: "tka992747"
    permissionsToDrop:
    [
      {permission: "ROLE: 1"},
      {permission: "ROLE: 2"},
      {permission: "ROLE: 3"}
    ]
    permissionsToAdd:
    [
      {permission: "ROLE: 1"},
      {permission: "ROLE: 2"},
      {permission: "ROLE: 3"}
    ]
  })
  {
    roles:permissions {
      permission
    }
  }
}