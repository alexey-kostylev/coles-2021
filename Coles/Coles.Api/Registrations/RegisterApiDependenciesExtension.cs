using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coles.Api.MusicBrainz;
using Coles.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coles.Api.Registrations
{
    public static class RegisterApiDependenciesExtension
    {
        public static IServiceCollection RegisterApiDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            return
                services
                .RegisterMapper()
                .RegisterServices(configuration)
                ;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration
                .GetSection(MusicBrainzSettings.MusicBrainzSettingsSectionName);

            if (!section.Exists())
            {
                throw new InvalidOperationException($"config section '{MusicBrainzSettings.MusicBrainzSettingsSectionName}' does not exist");
            }

            var musicBrainzSettings = section
                .Get<MusicBrainzSettings>();

            services
                .AddSingleton(musicBrainzSettings)
                .AddScoped<IMusicService, MusicService>()
                .AddHttpClient<IMusicBrainzClient, MusicBrainzClient>(c =>
                {
                    c.BaseAddress = new Uri(musicBrainzSettings.BaseUrl);
                    c.DefaultRequestHeaders.Add("User-Agent", "Coles test");
                })
                ;

            return services;
        }

        public static IServiceCollection RegisterMapper(this IServiceCollection services)
        {
            services
                .AddAutoMapper(typeof(RegisterApiDependenciesExtension));
            return services;
        }
    }
}
