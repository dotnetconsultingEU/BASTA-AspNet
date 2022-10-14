# Eine Abfrage mit Paging
# [UseOffsetPaging]
query{
  AllUsers:usersDirect(take: 3 skip: 0, where: {name: {eq: "u000012"}, email: {eq: "aaa"}}, order: {name: ASC, lastName:DESC})
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