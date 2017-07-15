# MeetingManagementServer

Web API for managing meetings based on ASP.NET Core

## Show all partners

  Returns json data about all partners.

  `GET` **/api/partners**
  
  Success response:

  ```
  [
    {
      "id": 1,
      "name": "John Doe",
      "email": "john.doe@jd.com",
      "country": "USA",
      "availableDates": [
        "2017-01-09T00:00:00",
        "2017-01-10T00:00:00"
      ]
    },
    {
      "id": 2,
      "name": "Mary Smith",
      "email": "mary.smith@ms.com",
      "country": "UK",
      "availableDates": [
        "2017-03-20T00:00:00",
        "2017-03-22T00:00:00"
      ]
    }
  ]
  ```
   
  ## Show partner

  Returns json data about a specific partner.

  `GET` **/api/partners/[id]**
  
  URL Params:

  **id** `integer` - required

  Success response:
  
  ```
  {
    "id": 1,
    "name": "John Doe",
    "email": "john.doe@jd.com",
    "country": "USA",
    "availableDates": [
      "2017-01-10T00:00:00",
      "2017-01-09T00:00:00"
    ]
  } 
  ```
  
  ## Create partner

  Creates a new partner. Returns json data about the created partner.

  `POST` **/api/partners**
  
  Request body:

  ```
  {
    "name": "John Doe",
    "email": "john.doe@jd.com",
    "country": "USA",
    "availableDates": [
      "2017-01-10T00:00:00",
      "2017-01-09T00:00:00"
    ]
  } 
  ```

  Success response:
  
  ```
  {
    "id": 1,
    "name": "John Doe",
    "email": "john.doe@jd.com",
    "country": "USA",
    "availableDates": [
      "2017-01-10T00:00:00",
      "2017-01-09T00:00:00"
    ]
  } 
  ```
  
  Error response:
  * "Partner is not specified correctly"
  * "Country is not specified correctly"
  * "To update an existing partner, use PUT request"
  * "Email is not unique"
