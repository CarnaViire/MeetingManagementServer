# Meeting API

The list of countries is obtained from the partners.
In each country, the meeting should take the two consecutive days with maximal attendance of partners from this country. Each partner should be available for all days of the meeting. If there are several dates with maximal attendance, the earliest one is chosen.

## Build all meeting dates

Compute meeting dates for all countries

`GET` **/api/meeting/BuildAll**

Success response example:

```
[
  {
    "country": "USA",
    "startDate": "2017-01-09T00:00:00",
    "attendees": [
      {
        "id": 1,
        "name": "John Doe",
        "email": "john.doe@jd.com"
      },
      {
        "id": 10,
        "name": "Kathy Abrams",
        "email": "kathy.abrams@ka.com"
      }
    ]
  },
  {
    "country": "UK",
    "startDate": "2017-03-20T00:00:00",
    "attendees": [
      {
        "id": 2,
        "name": "Mary Smith",
        "email": "mary.smith@ms.com"
      }
    ]
  },
  {
    "country": "CANADA",
    "startDate": null,
    "attendees": []
  }
]
```

## Build meeting date for country

Compute a meeting date for the specified country

`GET` **/api/meeting/Build?country=[country]**

URL Params:

**country** `string` - required

Success response example:

```
{
  "country": "UK",
  "startDate": "2017-03-20T00:00:00",
  "attendees": [      
      {
        "id": 2,
        "name": "Mary Smith",
        "email": "mary.smith@ms.com"
      }
    ]
}
```

  Error response examples:
  * `404` - no country found with the specified name