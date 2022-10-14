# Create user
mutation{
  addOrUpdateUser(user: {
    userAccount:{
      uNumber: "tka992747"
      firstname: "Thorsten"
      lastname: "Kansy"
      email: "tkansy@dotnetconsulting.eu"
      culture: "de-DE"
    }
    requestedPlants:{
      code:"dnc1"
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