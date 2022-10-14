query{
  Result3:usersDirect(where: {objectId: {gt: 4}}, order: {name: ASC})
  {
    objectId
    name
    roles{
      name
      objectId
    }
    userPlantCodes{
      code
    }
  }
}