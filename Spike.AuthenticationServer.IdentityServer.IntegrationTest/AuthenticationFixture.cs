using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Spike.AuthenticationServer.IdentityServer.IntegrationTest
{
    public class AuthenticationFixture : IDisposable
    {
        public readonly TestServer server;
        public IConfiguration Configuration;
        private string configFileName = "appsettings.json";

        public AuthenticationFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(GetFullPathToTestConfigFile())
                .AddJsonFile(configFileName, optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services => services.AddAutofac())
                .UseUrls(Configuration["JwtIssuer"])
                .UseConfiguration(Configuration)
                .UseStartup<Startup>());
        }

        public void Dispose()
        {
            server.Dispose();
        }

        private string GetFullPathToTestConfigFile()
        {
            return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".."));
        }
    }
}
