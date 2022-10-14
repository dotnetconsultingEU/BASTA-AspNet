# Create
mutation{
  addOrUpdateUser(user: {
    userAccount:{
      uNumber: "tka992747"
    }
    requestedPermissions:
    [
      {permission: "ROLE: 1"},
      {permission: "ROLE: 2"},
      {permission: "ROLE: 3"}
    ]
  })
  { 
	result 
  }
}
