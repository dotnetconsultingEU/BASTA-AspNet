# Abfrage mit Sub-Filter
query{
  Result2:usersDirect(where: {roles: { some: {name: {eq: "ROLE: 1"}}}})
  {
    name
    roles {
      name
    }
  }
}