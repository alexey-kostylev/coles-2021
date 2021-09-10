using System.Threading.Tasks;
using Coles.Api.Models;

namespace Coles.Api.Services
{
    public interface IMusicService
    {
        Task<ArtistResponse> GetArtistsOrReleases(string artistOrBandName);
    }
}