# Eine Abfrage mit Paging
# [UsePaging]
query{
  AllUsers:usersDirect(first: 3, order: {name: ASC})
  {
    nodes{
      uNumber: name
      name
      firstName
      lastName
      email
      culture: resourceId
      permissions: roles{
        name
      }
      plants: userPlantCodes {
        code
      }
    }
  }
}