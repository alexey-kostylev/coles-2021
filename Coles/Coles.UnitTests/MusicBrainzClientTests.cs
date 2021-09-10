using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coles.Api.MusicBrainz;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using Moq.Protected;
using Xunit;

namespace Coles.UnitTests
{
    public class MusicBrainzClientTests : TestBase
    {
        private Mock<HttpMessageHandler> _mockHttpHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        private MusicBrainzClient CreateService()
        {
            return new MusicBrainzClient(new HttpClient(_mockHttpHandler.Object)
            {
                BaseAddress = new Uri("http://localhist:6000")
            });
        }


        [Fact]
        public void NameIsNullShouldThrowException()
        {
            var service = CreateService();
            Func<Task<SearchResponse>> act = async () => await service.SearchArtists(null, 0);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public async void ShouldSearch()
        {
            var json = Properties.Resources.search;
            HttpRequestMessage reqMessage = null;

            _mockHttpHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .Callback<HttpRequestMessage, CancellationToken>((msg, token) => reqMessage = msg)
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(json),
               })
               .Verifiable();

            var service = CreateService();

            var data = await service.SearchArtists("test", 100);
            data.Count.Should().Be(3);
            data.Artists.Should().HaveCount(3);

            reqMessage.Should().NotBeNull();
            reqMessage.Method.Should().Be(HttpMethod.Get);
            reqMessage.RequestUri.Segments.Last().Should().Be("artist");

            var qryParams = QueryHelpers.ParseQuery(reqMessage.RequestUri.Query);
            qryParams["query"].ToArray().Any(x => x == "\"test\"").Should().BeTrue();
            qryParams["offset"].ToArray().Any(x => x == "100").Should().BeTrue();
            qryParams["fmt"].ToArray().Any(x => x == "json").Should().BeTrue();
        }

        [Fact]
        public void ArtistIsNullShouldThrowException()
        {
            var service = CreateService();
            Func<Task<LookupReleaseResponse>> act = async () => await service.LookupReleases(null, 0);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public async void ShouldLookupReleases()
        {
            var json = Properties.Resources.lookup_response;
            HttpRequestMessage reqMessage = null;

            _mockHttpHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .Callback<HttpRequestMessage, CancellationToken>((msg, token) => reqMessage = msg)
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(json),
               })
               .Verifiable();

            var service = CreateService();

            var data = await service.LookupReleases("artist-id", 100);
            data.ReleaseCount.Should().Be(2);
            data.Releases.Should().HaveCount(2);

            reqMessage.Should().NotBeNull();
            reqMessage.Method.Should().Be(HttpMethod.Get);
            reqMessage.RequestUri.Segments.Last().Should().Be("release");

            var qryParams = QueryHelpers.ParseQuery(reqMessage.RequestUri.Query);
            qryParams["artist"].ToArray().Any(x => x == "artist-id").Should().BeTrue();
            qryParams["offset"].ToArray().Any(x => x == "100").Should().BeTrue();
            qryParams["fmt"].ToArray().Any(x => x == "json").Should().BeTrue();
        }
    }
}
