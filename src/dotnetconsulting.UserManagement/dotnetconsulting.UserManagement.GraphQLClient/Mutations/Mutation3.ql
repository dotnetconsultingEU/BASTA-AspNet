# Mehrfache Mutions ohne Body, dafür mit Parameter
mutation{
  Job1:startJob(jobId: 47)
  {
    id
    name
  },
  Job2:startJob(jobId: 4711)
  {
    id
    name
  }
}