# Coles enginnering test
ASP.NET 5 API project to fetch artists or relases from [http://musicbrainz.org](http://musicbrainz.org)

## Endpoints:
1. api/artists. [GET] ?artistOrBandName=Alexey%20Krivdin. Parameter: "artistOrBandName" - a required parameter. If null then 400 is returned. For successfull calls 200 is returned with a json payload
2. swagger - swagger endpoint
3. health - health check endpoint. Makes sure that MusicBrainz is accessable

## Implementation
API makes a serach by artist name and fetches all found entities. If entities more than one all entities are returned. If there is a single artist, all releases are fetched for a given artist and returned back.

MusicBrainz uses paging and all queries return a limited number of records. If total records is higher than number of fetched records that means more records needed to be fetched.
In that case API makes a loop fetching records until it reaches total records or an empty collection is returned. 
For performance reason the total number of records is limited to 10000 both for artists and releases.
Also, loop sanity check is implemented to prevent from falling into an indefinite loop. TO achieve that a number of iterations is limited to 10000

## testing
1. Unit test project is included to test business logic
2. Intergration test project is included to test a real http query to MusicBrainz


## Payload Example

multiple artists response example:
```json
{
  "responseType": "Artists",
  "artists": [
    {
      "id": "cc52a59d-16d9-4552-b30d-2aa305374f2c",
      "type": "Person",
      "type-id": "b6e035f4-3ce9-331c-97df-83397230b0df",
      "score": 100,
      "name": "Alexey Krivdin",
      "sort-name": "Krivdin, Alexey",
      "country": null,
      "area": null,
      "isnis": [],
      "life-span": {
        "ended": null,
        "begin": null
      },
      "aliases": [],
      "tags": []
    },
    {
      "id": "040ce6f5-d456-488d-bfa0-c0e39046b2bf",
      "type": "Group",
      "type-id": "e431f5f6-b5d2-343d-8b36-72607fffb74b",
      "score": 85,
      "name": "pavluchenko, Alexey Krivdin",
      "sort-name": "pavluchenko, Alexey Krivdin",
      "country": null,
      "area": null,
      "isnis": [],
      "life-span": {
        "ended": null,
        "begin": null
      },
      "aliases": [],
      "tags": []
    }
  ],
  "releases": []
}
```

single artist, list of releases:
```json
{
  "responseType": "Releases",
  "artists": [],
  "releases": [
    {
      "release-events": [
        {
          "date": "2019-12-25",
          "area": null
        }
      ],
      "disambiguation": "",
      "text-representation": {
        "language": "rus",
        "script": "Cyrl"
      },
      "packaging": null,
      "status-id": "4e304316-386d-3409-af2e-78857eec5cfe",
      "date": "2019-12-25",
      "status": "Official",
      "cover-art-archive": {
        "artwork": true,
        "front": true,
        "darkened": false,
        "count": 1,
        "back": false
      },
      "title": "Река",
      "quality": "normal",
      "id": "e629375b-8acf-4d33-b844-e33495c41ae3",
      "country": null,
      "asin": null,
      "packaging-id": null,
      "barcode": null
    }
  ]
}
```




