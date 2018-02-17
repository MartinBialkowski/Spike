using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;

namespace Spike.AuthenticationServer.IdentityServer.IntegrationTest
{
    public class AuthenticationFixture : IDisposable
    {
        public readonly TestServer server;

        public AuthenticationFixture()
        {
            server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
        }

        public void Dispose()
        {
            server.Dispose();
        }
    }
}
