# Eine Abfrage mit Parameter
query{
  singleUser(uNumber: "u000012") 
  { 
    uNumber
    username
    firstname    
    lastname
    email
    culture
    permissions{
      permission
    }
    plants {
      code
    }
  }
}