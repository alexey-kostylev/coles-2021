using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coles.Api.Models;
using Coles.Api.MusicBrainz;
using brainzModels=Coles.Api.MusicBrainz.Models;

namespace Coles.Api.Services
{
    public class MusicService : IMusicService
    {
        private readonly IMusicBrainzClient _musicClient;
        private readonly IMapper _mapper;

        /// <summary>
        /// Max number of records allowed to fecth from server
        /// </summary>
        private const int MaxRecordCount = 10000;

        public MusicService(IMusicBrainzClient musicClient, IMapper mapper)
        {
            _musicClient = musicClient;
            _mapper = mapper;
        }

        public async Task<ArtistResponse> GetArtistsOrReleases(string artistOrBandName)
        {
            var artists = await FetchAllRecords<brainzModels.Artist>(async (int offset) =>
            {
                var data = await _musicClient.SearchArtists(artistOrBandName, offset);
                if (data == null)
                {
                    throw new InvalidOperationException($"no search data received for {nameof(artistOrBandName)}={artistOrBandName}");
                }
                return new RecordPackage<brainzModels.Artist>
                {
                    TotalRecords = data.Count,
                    Records = data.Artists,
                };
            });

            if (artists.Count == 1)
            {
                var artistId = artists.Single().Id;
                var releases = await FetchAllRecords<brainzModels.Release>(async (int offset) =>
                {
                    var data = await _musicClient.LookupReleases(artistId, offset);
                    if (data == null)
                    {
                        throw new InvalidOperationException($"no search data received for {nameof(artistId)}={artistId}");
                    }
                    return new RecordPackage<brainzModels.Release>
                    {
                        TotalRecords = data.ReleaseCount,
                        Records = data.Releases,
                    };
                });

                return ArtistResponse.CreateResponseForReleases(_mapper.Map<ICollection<Release>>(releases));
            }

            return ArtistResponse.CreateResponseForArtists(_mapper.Map<ICollection<Artist>>(artists));
        }

        /// <summary>
        /// fetches records from musicbrainz using paging. Max record limit - 10000
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fetch"></param>
        /// <returns></returns>
        private async Task<ICollection<T>> FetchAllRecords<T>(Func<int, Task<RecordPackage<T>>> fetch)
        {
            var package = await fetch(0);

            if (package.Records == null)
            {
                throw new InvalidOperationException("fetched is null");
            }

            // check if all artists are fetched in the first run
            if (package.TotalRecords == package.Records.Count)
            {
                return package.Records;
            }

            //begin fetching by pages
            var result = new List<T>(package.Records);
            int offset = package.Records.Count;
            int counter = 0;
            while (true)
            {
                var fetched = await fetch(offset);
                if (fetched == null)
                {
                    throw new InvalidOperationException("fetched is null");
                }

                offset += fetched.Records.Count;
                result.AddRange(fetched.Records);

                //check if no records left, if all records fetched of somehow entered an infinite loop
                if (!fetched.Records.Any() || result.Count >= fetched.TotalRecords || result.Count > MaxRecordCount)
                {
                    break;
                }
                counter++;
                // sanity check in case we entered an endless loop
                if (counter > MaxRecordCount)
                {
                    throw new InvalidOperationException($"Endless loop detected. Iteration count={counter}");
                }
            }

            return result;
        }

        private record RecordPackage<T>
        {
            public int TotalRecords { get; set; }

            public ICollection<T> Records { get; set; }
        }
    }
}
