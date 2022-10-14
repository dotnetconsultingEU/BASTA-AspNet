# Abfrage mit Filter
# No Paging
query{
  AllUsers:usersDirect(where: {name: {eq: "u000012"}})
  {
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