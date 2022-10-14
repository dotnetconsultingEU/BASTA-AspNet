# Eine Abfrage mit Paging
# [UseOffsetPaging]
query{
  AllUsers:usersDirect(take: 3 skip: 0, order: {name: ASC})
  {
    items{
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
    pageInfo {
      hasNextPage
      hasPreviousPage
    }
  }
}