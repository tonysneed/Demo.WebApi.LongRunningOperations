# Demo: Web API Long Running Operations

Long running Web API operations that can report progress and be cancelled.

## Browser or Postman

- http://localhost:54518/api/values/start
  + returns guid
- http://localhost:54518/api/values/progress/guid
  + returns number indicating progress
  + returns 0 if cancelled
  + returns -1 if completed
- http://localhost:54518/api/values/cancel/guid
  + cancels operation
- http://localhost:54518/api/values/result/guid
  + returns empty array if not complete
  + returns results if complete
