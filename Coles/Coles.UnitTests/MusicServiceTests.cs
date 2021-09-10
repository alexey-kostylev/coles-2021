using System;
using System.Collections.Generic;
using System.Linq;
using Coles.Api.Models;
using Coles.Api.MusicBrainz;
using Coles.Api.Services;
using FluentAssertions;
using Moq;
using Xunit;
using musicModels = Coles.Api.MusicBrainz.Models;

namespace Coles.UnitTests
{
    public class MusicServiceTests : TestBase
    {
        private Mock<IMusicBrainzClient> _mockMusicClient = new();

        private MusicService CreateService()
        {
            return new MusicService(_mockMusicClient.Object, Mapper);
        }

        private static musicModels.Artist[] GenerateArtists(int count)
        {
            return Enumerable.Range(1, count).Select(x => new musicModels.Artist { Id = $"{x}", Name = $"artist-{x}" }).ToArray();
        }

        private static List<musicModels.Release> GenerateReleases(int count)
        {
            return Enumerable.Range(1, count).Select(x => new musicModels.Release { Id = $"{x}", Title = $"title-{x}" }).ToList();
        }

        [Fact]
        public async void ShouldGetMultipleArtists()
        {
            _mockMusicClient.Setup(x => x.SearchArtists("test", 0))
                .ReturnsAsync(new SearchResponse
                {
                    Count = 2,
                    Artists = GenerateArtists(2)
                });

            var service = CreateService();
            var response = await service.GetArtistsOrReleases("test");
            response.Artists.Should().HaveCount(2);

            _mockMusicClient.Verify(x => x.SearchArtists(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            _mockMusicClient.Verify(x => x.LookupReleases(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async void ShouldGetMultipleArtistsUsingPaging()
        {
            var artists = GenerateArtists(7);

            const int PageSize = 3;

            // should call api 3 times with page size 3
            _mockMusicClient.Setup(x => x.SearchArtists("test", It.IsAny<int>()))
                .ReturnsAsync((string _, int offset) => {
                    return new SearchResponse
                    {
                        Count = 7,
                        Artists = artists.Skip(offset).Take(PageSize).ToArray()
                    };
                });

            var service = CreateService();
            var response = await service.GetArtistsOrReleases("test");

            var expectedArtists = Mapper.Map<ICollection<Artist>>(artists);

            response.Artists.Should().BeEquivalentTo(expectedArtists);
            response.ResponseType.Should().Be(ResponseType.Artists);

            _mockMusicClient.Verify(x => x.SearchArtists(It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(3));
            _mockMusicClient.Verify(x => x.LookupReleases(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async void ShouldGetSingleArtistsAndFetchReleases()
        {
            _mockMusicClient.Setup(x => x.SearchArtists("test", 0))
                .ReturnsAsync(new SearchResponse
                {
                    Count = 1,
                    Artists = GenerateArtists(1)
                });

            _mockMusicClient.Setup(x => x.LookupReleases("1", 0))
                .ReturnsAsync(new LookupReleaseResponse
                {
                    ReleaseCount = 2,
                    Releases = GenerateReleases(2)
                });

            var service = CreateService();
            var response = await service.GetArtistsOrReleases("test");
            response.Artists.Should().BeEmpty();
            response.Releases.Should().HaveCount(2);
            response.ResponseType.Should().Be(ResponseType.Releases);

            _mockMusicClient.Verify(x => x.SearchArtists(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            _mockMusicClient.Verify(x => x.LookupReleases(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }
    }
}
