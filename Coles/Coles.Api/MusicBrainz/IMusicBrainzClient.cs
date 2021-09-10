using System.Threading.Tasks;

namespace Coles.Api.MusicBrainz
{
    public interface IMusicBrainzClient
    {
        Task<SearchResponse> SearchArtists(string artistOrBandName, int offset);

        Task<LookupReleaseResponse> LookupReleases(string artistId, int offset);
    }
}