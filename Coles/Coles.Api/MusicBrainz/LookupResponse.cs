using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Coles.Api.MusicBrainz.Models;

namespace Coles.Api.MusicBrainz
{
    public class LookupReleaseResponse
    {
        [JsonPropertyName("release-offset")]
        public int ReleaseOffset { get; set; }

        [JsonPropertyName("release-count")]
        public int ReleaseCount { get; set; }
        public List<Release> Releases { get; set; }
    }


}
