# Multiple Users und Permissions
# No paging
query
{
  allRoles: rolesDirect(order: { name: ASC})
  {
        name
        users {
            name
            firstName
          lastName
        }
    }
# Trennliene ~ 715
  allUsers: usersDirect(order: { name: ASC})
  {
    uNumber: name
    name
    firstName
    lastName
    email
  }
}