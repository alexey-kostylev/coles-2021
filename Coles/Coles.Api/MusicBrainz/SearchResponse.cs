using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coles.Api.MusicBrainz.Models;

namespace Coles.Api.MusicBrainz
{
    public class SearchResponse
    {
        public DateTime Created { get; set; }
        public int Count { get; set; }
        public int Offset { get; set; }
        public ICollection<Artist> Artists { get; set; }
    }
}
