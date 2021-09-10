using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coles.Api;
using Coles.Api.MusicBrainz;
using Coles.Api.Registrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coles.IntegrationTests
{
    public class TestBase
    {
        private static readonly IConfigurationRoot _cfg;
        protected IServiceProvider ServiceProvider;

        protected IMusicBrainzClient MusicClient;

        static TestBase()
        {
            var currentFile = new Uri(typeof(TestBase).Assembly.Location).LocalPath;
            var basePath = Path.GetDirectoryName(currentFile);
            _cfg = GetConfigurationRoot(basePath);
        }

        protected TService GetService<TService>()
        {
            return ServiceProvider.GetRequiredService<TService>();
        }

        protected static TService BuildServiceProviderAndGetService<TService>()
        {
            return BuildServiceProvider().GetRequiredService<TService>();
        }

        protected static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            services
                .RegisterServices(_cfg)
                ;

            return services.BuildServiceProvider();
        }

        public static IConfigurationRoot GetConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("test-appsettings.json", optional: true)
                .Build();
        }
    }
}
