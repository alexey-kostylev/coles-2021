using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coles.Api.MusicBrainz.Models
{
    public class ReleaseArea
    {
        public string Id { get; set; }

        [JsonPropertyName("iso-3166-1-codes")]
        public List<string> Iso31661Codes { get; set; }
        public object Type { get; set; }
        public string Name { get; set; }
        public string Disambiguation { get; set; }

        [JsonPropertyName("sort-name")]
        public string SortName { get; set; }

        [JsonPropertyName("type-id")]
        public object TypeId { get; set; }

        [JsonPropertyName("iso-3166-3-codes")]
        public List<string> Iso31663Codes { get; set; }
    }

    public class ReleaseEvent
    {
        public string Date { get; set; }
        public ReleaseArea Area { get; set; }
    }

    public class TextRepresentation
    {
        public string Language { get; set; }
        public string Script { get; set; }
    }

    public class CoverArtArchive
    {
        public bool Artwork { get; set; }
        public bool Front { get; set; }
        public bool Darkened { get; set; }
        public int Count { get; set; }
        public bool Back { get; set; }
    }

    public class Release
    {
        [JsonPropertyName("release-events")]
        public List<ReleaseEvent> ReleaseEvents { get; set; }
        public string Disambiguation { get; set; }

        [JsonPropertyName("text-representation")]
        public TextRepresentation TextRepresentation { get; set; }
        public string Packaging { get; set; }

        [JsonPropertyName("status-id")]
        public string StatusId { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }

        [JsonPropertyName("cover-art-archive")]
        public CoverArtArchive CoverArtArchive { get; set; }
        public string Title { get; set; }
        public string Quality { get; set; }
        public string Id { get; set; }
        public string Country { get; set; }
        public string Asin { get; set; }

        [JsonPropertyName("packaging-id")]
        public string PackagingId { get; set; }
        public string Barcode { get; set; }
    }
}
