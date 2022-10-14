# Abfrage mit Sortierung
# No paging
query{
  allRoles:rolesDirect(order: {name: ASC})
  {
    name
    users {
      name
      firstName
      lastName
    }
  }
}