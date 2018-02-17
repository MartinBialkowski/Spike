using Autofac.Extensions.DependencyInjection;
using EFCoreSpike5.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Spike.WebApi.Types.DTOs;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Spike.WebApi.IntegrationTest
{
    public class ControllerFixture : IDisposable
    {
        public readonly TestServer server;
        public readonly EFCoreSpikeContext context;
        public string authenticationToken;
        private HttpClient client;
        private string configFileName = "appsettings.json";
        public ControllerFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(GetFullPathToTestConfigFile())
                .AddJsonFile(configFileName, optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<EFCoreSpikeContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            context = new EFCoreSpikeContext(optionsBuilder.Options);

            server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services => services.AddAutofac())
                .UseConfiguration(configuration)
                .UseStartup<Startup>());

            var getTokenTask = GetAuthorizationToken();
            authenticationToken = getTokenTask.Result;
        }
        public void Dispose()
        {
            authenticationToken = null;
            server.Dispose();
            context.Dispose();
        }

        private async Task<string> GetAuthorizationToken()
        {
            string requestMediaType = "application/json";
            using (client = server.CreateClient())
            {
                var loginUrl = "api/account/login";
                var userCredential = new UserDTO()
                {
                    Email = "embe2sc@gmail.com",
                    Password = "TestMB123!"
                };
                string userJson = JsonConvert.SerializeObject(userCredential);
                var content = new StringContent(userJson, Encoding.UTF8, requestMediaType);
                HttpResponseMessage httpResponse = await client.PostAsync(loginUrl, content);
                var response = await httpResponse.Content.ReadAsStringAsync();
                return response.Trim('"');
            }
        }

        private string GetFullPathToTestConfigFile()
        {
            return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".."));
        }
    }
}
