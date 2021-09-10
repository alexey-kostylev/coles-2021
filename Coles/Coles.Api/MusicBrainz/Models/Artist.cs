using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coles.Api.MusicBrainz.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class LifeSpan
    {
        public object Ended { get; set; }
        public string Begin { get; set; }
    }

    public class Area
    {
        public string Id { get; set; }
        public string Type { get; set; }

        [JsonPropertyName("type-id")]
        public string TypeId { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("sort-name")]
        public string SortName { get; set; }

        [JsonPropertyName("life-span")]
        public LifeSpan LifeSpan { get; set; }
    }

    public class Alias
    {
        [JsonPropertyName("sort-name")]
        public string SortName { get; set; }

        [JsonPropertyName("type-id")]
        public string TypeId { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        public string Type { get; set; }
        public object Primary { get; set; }

        [JsonPropertyName("begin-date")]
        public object BeginDate { get; set; }

        [JsonPropertyName("end-date")]
        public object EndDate { get; set; }
    }

    public class Tag
    {
        public int Count { get; set; }
        public string Name { get; set; }
    }

    public class Artist
    {
        public string Id { get; set; }
        public string Type { get; set; }

        [JsonPropertyName("type-id")]
        public string TypeId { get; set; }
        public int Score { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("sort-name")]
        public string SortName { get; set; }
        public string Country { get; set; }
        public Area Area { get; set; }
        public List<string> Isnis { get; set; }

        [JsonPropertyName("life-span")]
        public LifeSpan LifeSpan { get; set; }
        public List<Alias> Aliases { get; set; }
        public List<Tag> Tags { get; set; }

        public override string ToString() => $"[{Id}], {Name}, type={Type}";
    }


}
