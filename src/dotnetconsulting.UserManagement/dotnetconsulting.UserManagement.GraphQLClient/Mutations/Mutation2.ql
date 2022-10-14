# Mehrfache Mutions ohne Body, dafür mit Parameter
mutation{
  Job1:startJob(jobId: 47) { name }
  Job2:startJob(jobId: 48) { name }
}