using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Coles.Api.Exceptions;
using Microsoft.AspNetCore.WebUtilities;

namespace Coles.Api.MusicBrainz
{
    /// <summary>
    /// API Client for <see href="https://musicbrainz.org/doc/MusicBrainz_API#Browse">MusicBrainz</see>
    /// </summary>
    public class MusicBrainzClient : IMusicBrainzClient
    {
        private readonly HttpClient _httpClient;

        private const string ArtistEntityName = "artist";
        private const string ReleaseEntityName = "release";

        public MusicBrainzClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Search by artist. Search ewngine is based on <see href="https://musicbrainz.org/doc/MusicBrainz_API/Search"/>
        /// </summary>
        /// <param name="artistOrBandName"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<SearchResponse> SearchArtists(string artistOrBandName, int offset)
        {
            if (string.IsNullOrEmpty(artistOrBandName))
            {
                throw new ArgumentException($"'{nameof(artistOrBandName)}' cannot be null or empty.", nameof(artistOrBandName));
            }

            // building url of this kind: http://musicbrainz.org/ws/2/artist/?query=alexey&offset=xx&fmt=json

            /*
             *
             * in case that by artist is meant only a person or group the search can be limited to a person or a group
            // search by name and limit type to a person or group
            var queryValue = $"\"{artistOrBandName.Trim()}\" AND (type:group OR type:person)";
            */

            // by default search by any artist and
            var queryValue = $"\"{artistOrBandName.Trim()}\"";
            var qryParams = new Dictionary<string, string>()
            {
                {"query", queryValue },
                {"offset", $"{ offset}" },
                {"fmt", "json" }
            };
            var url = QueryHelpers.AddQueryString(ArtistEntityName, qryParams);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await ThrowApiClientExceptionAsync(response).ConfigureAwait(false);
            }

            var stringData = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<SearchResponse>(stringData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
            return data;
        }

        /// <summary>
        /// Creates <see cref="ApiClientException"/> from HttpResponseMessage and throws lateron
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        protected async Task ThrowApiClientExceptionAsync(HttpResponseMessage responseMessage)
        {
            var content = await responseMessage.Content.ReadAsStringAsync() ?? responseMessage.ReasonPhrase;
            throw new ApiClientException(content, responseMessage);
        }

        public async Task<LookupReleaseResponse> LookupReleases(string artistId, int offset)
        {
            if (string.IsNullOrEmpty(artistId))
            {
                throw new ArgumentException($"'{nameof(artistId)}' cannot be null or empty.", nameof(artistId));
            }
            // building url of this kind: http://musicbrainz.org/ws/2/release?artist=xxx&offset=yy&fmt=json
            var qryParams = new Dictionary<string, string>()
            {
                {ArtistEntityName, $"{artistId.Trim()}" },
                {"offset", $"{ offset}" },
                {"fmt", "json" }
            };
            var url = QueryHelpers.AddQueryString(ReleaseEntityName, qryParams);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await ThrowApiClientExceptionAsync(response).ConfigureAwait(false);
            }

            var stringData = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<LookupReleaseResponse>(stringData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
            return data;
        }
    }
}
