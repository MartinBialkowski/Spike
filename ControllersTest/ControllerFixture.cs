using Autofac.Extensions.DependencyInjection;
using EFCoreSpike5.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Spike.WebApi.IntegrationTest
{
    public class ControllerFixture : IDisposable
    {
        public readonly TestServer server;
        public readonly EFCoreSpikeContext context;
        public string authenticationToken;
        public IConfiguration Configuration;
        private string configFileName = "appsettings.json";
        public ControllerFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(GetFullPathToTestConfigFile())
                .AddJsonFile(configFileName, optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<EFCoreSpikeContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            context = new EFCoreSpikeContext(optionsBuilder.Options);

            server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services => services.AddAutofac())
                .UseConfiguration(Configuration)
                .UseStartup<Startup>());

            var getTokenTask = GetAuthorizationToken();
            authenticationToken = getTokenTask.Result.AccessToken;
        }
        public void Dispose()
        {
        }

        private async Task<TokenResponse> GetAuthorizationToken()
        {
            TokenResponse tokenResponse;
            var clientId = Configuration["SpikeClientId"];
            var username = Configuration["SpikeTestUsername"];
            var password = Configuration["SpikeTestPassword"];
            var secret = Configuration["SpikeSecret"];
            var apiScope = Configuration["SpikeAudience"];
            DiscoveryResponse discovery;

            using (var discoveryClient = new DiscoveryClient(Configuration["JwtIssuer"]))
            {
                discovery = await discoveryClient.GetAsync();
            }

            using (var tokenClient = new TokenClient(discovery.TokenEndpoint, clientId, secret))
            {
                return tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(username, password, apiScope);
            }
        }

        private string GetFullPathToTestConfigFile()
        {
            return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".."));
        }
    }
}
