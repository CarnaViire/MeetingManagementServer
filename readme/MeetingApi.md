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
    "attendees": []
  },
  {
    "country": "UK",
    "startDate": "2017-03-20T00:00:00",
    "attendees": []
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
  "attendees": []
}
```

  Error response example:
  * `Country is not specified correctly` - country is null or there is no country with specified name