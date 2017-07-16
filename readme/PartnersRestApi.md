# Partners REST API

REST API for specifying and managing partners

## Show all partners

  Returns json data about all partners.

  `GET` **/api/partners**

  Success response example:

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

  Success response example:

  ```
  {
    "id": 1,
    "name": "John Doe",
    "email": "john.doe@jd.com",
    "country": "USA",
    "availableDates": [
      "2017-01-09T00:00:00",
      "2017-01-10T00:00:00"
    ]
  } 
  ```

## Create partner

  Creates a new partner. Returns json data about the created partner.

  `POST` **/api/partners**

  Request body example:

  ```
  {
    "name": "John Doe",
    "email": "john.doe@jd.com",
    "country": "USA",
    "availableDates": [
      "2017-01-09",
      "2017-01-10"
    ]
  } 
  ```

  Success response example:

  ```
  {
    "id": 1,
    "name": "John Doe",
    "email": "john.doe@jd.com",
    "country": "USA",
    "availableDates": [
      "2017-01-09T00:00:00",
      "2017-01-10T00:00:00"
    ]
  } 
  ```

  Error response example:
  * `Partner is not specified correctly` - could not parse JSON
  * `Country is not specified correctly` - country is null or whitespace
  * `To update an existing partner, use PUT request` - non-zero Id specified in JSON
  * `Email is not unique` - there is already a partner with specified email

## Update partner

  Updates an existing partner. Returns json data about the updated partner.

  `PUT` **/api/partners/[id]**

  URL Params:

  **id** `integer` - required

  Request body example:

  ```
  {
    "name": "John Doe Jr.",
    "email": "john.doe.jr@jd.com",
    "country": "USA",
    "availableDates": [
      "2017-01-09",
      "2017-01-10",
      "2017-01-11"
    ]
  } 
  ```

  Success response example:

  ```
  {
    "id": 1,
    "name": "John Doe Jr.",
    "email": "john.doe.jr@jd.com",
    "country": "USA",
    "availableDates": [
      "2017-01-09T00:00:00",
      "2017-01-10T00:00:00",
      "2017-01-11T00:00:00"
    ]
  } 
  ```

  Error response example:
  * `Partner is not specified correctly` - could not parse JSON
  * `Country is not specified correctly` - country is null or whitespace
  * `To create a new partner, use POST request` - zero Id specified in URL
  * `Email is not unique` - there is already a partner with specified email

## Delete partner

  Deletes a partner. Returns json data about the deleted partner.

  `DELETE` **/api/partners/[id]**

  URL Params:

  **id** `integer` - required

  Success response example:

  ```
  {
    "id": 1,
    "name": "John Doe Jr.",
    "email": "john.doe.jr@jd.com",
    "country": "USA",
    "availableDates": [
      "2017-01-09T00:00:00",
      "2017-01-10T00:00:00",
      "2017-01-11T00:00:00"
    ]
  } 
  ```

## Delete all partners

  Deletes all partners and countries.

  `DELETE` **/api/partners**