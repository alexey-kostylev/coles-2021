using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coles.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Coles.Api.HealthChecks
{
    /// <summary>
    /// calls musicBrainz with a new guid to make sure it is reachable
    /// </summary>
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly IServiceScopeFactory _serviceProvider;

        public ApiHealthCheck(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceProvider = serviceScopeFactory;


        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var healthy = false;
            using (var scope = _serviceProvider.CreateScope())
            {
                var musicService = scope.ServiceProvider.GetRequiredService<IMusicService>();
                var response = await musicService.GetArtistsOrReleases($"{Guid.NewGuid()}");

                healthy = response.Artists.Count == 0 && response.Releases.Count == 0;
            }

            if (healthy)
            {
                return HealthCheckResult.Healthy($"API is healthy");
            }

            return HealthCheckResult.Unhealthy("An unhealthy result.");
        }
    }
}
