using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coles.Api.Models
{
    public class ArtistResponse
    {
        public static ArtistResponse CreateResponseForArtists(ICollection<Artist> artists)
        {
            return new ArtistResponse
            {
                ResponseType = ResponseType.Artists,
                Artists = artists
            };
        }

        public static ArtistResponse CreateResponseForReleases(ICollection<Release> artists)
        {
            return new ArtistResponse
            {
                ResponseType = ResponseType.Releases,
                Releases = artists
            };
        }

        protected ArtistResponse()
        {

        }

        public ResponseType ResponseType { get; private set; }

        public ICollection<Artist> Artists { get; set; } = new HashSet<Artist>();

        public ICollection<Release> Releases { get; set; } = new HashSet<Release>();
    }
}
