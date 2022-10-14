# Eine Abfrage mit Filter und Sortierung
query{
  Result:usersDirect(where: {objectId: {gt: 4}}, order: {name: ASC})
  {
    objectId
    name
    roles{
      name
      objectId
    }
  }
}