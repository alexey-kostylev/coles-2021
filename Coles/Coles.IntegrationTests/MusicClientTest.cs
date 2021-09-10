using System;
using Coles.Api.MusicBrainz;
using FluentAssertions;
using Xunit;

namespace Coles.IntegrationTests
{
    public class MusicClientTest : TestBase
    {
        [Fact]
        public async void ShouldSearchByArtist()
        {
            var client = BuildServiceProviderAndGetService<IMusicBrainzClient>();

            var response1 = await client.SearchArtists("alexey", 0);
            response1.Count.Should().BeGreaterOrEqualTo(215);
            response1.Artists.Should().HaveCount(25);

            var response2 = await client.SearchArtists("alexey", 100);
            response2.Count.Should().BeGreaterOrEqualTo(215);
            response2.Offset.Should().Be(100);
            response2.Artists.Should().HaveCount(25);
        }

        //
        [Fact]
        public async void ShouldLookupReleasesByArtistId()
        {
            var client = BuildServiceProviderAndGetService<IMusicBrainzClient>();

            var response = await client.LookupReleases("b2ad538c-23c4-4da9-a760-f2609c729bd3", 0);
            response.ReleaseCount.Should().Be(2);
            response.ReleaseOffset.Should().Be(0);
            response.Releases.Should().HaveCount(2);
        }
    }
}
