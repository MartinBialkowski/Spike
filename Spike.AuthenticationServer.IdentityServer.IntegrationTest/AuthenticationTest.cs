using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Spike.AuthenticationServer.IdentityServer.IntegrationTest
{
    public class AuthenticationTest : IClassFixture<AuthenticationFixture>
    {
        private AuthenticationFixture fixture;
        private string secret;
        private string apiScope;
        public AuthenticationTest(AuthenticationFixture fixture)
        {
            this.fixture = fixture;
            secret = this.fixture.Configuration["SpikeSecret"];
            apiScope = this.fixture.Configuration["SpikeAudience"];
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldAuthenticateWhenUsingResourceOwnerPassword()
        {
            // arrange
            TokenResponse tokenResponse;
            var clientId = fixture.Configuration["SpikeClientId"];
            var username = fixture.Configuration["SpikeTestUsername"];
            var password = fixture.Configuration["SpikeTestPassword"];
            var handler = fixture.server.CreateHandler();
            DiscoveryResponse discovery = await GetDiscoveryResponse(handler);
            // act
            using (var tokenClient = new TokenClient(discovery.TokenEndpoint, clientId, secret, handler))
            {
                tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(username, password, apiScope);
            }
            // assert
            Assert.False(tokenResponse.IsError);
            Assert.False(string.IsNullOrEmpty(tokenResponse.AccessToken));
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldAuthenticateWhenUsingClientCredential()
        {
            // arrange
            TokenResponse tokenResponse;
            var clientId = "client";
            var handler = fixture.server.CreateHandler();
            DiscoveryResponse discovery = await GetDiscoveryResponse(handler);
            // act
            using (var tokenClient = new TokenClient(discovery.TokenEndpoint, clientId, secret, handler))
            {
                tokenResponse = await tokenClient.RequestClientCredentialsAsync(apiScope);
            }
            // assert
            Assert.False(tokenResponse.IsError);
            Assert.False(string.IsNullOrEmpty(tokenResponse.AccessToken));
        }

        private async Task<DiscoveryResponse> GetDiscoveryResponse(HttpMessageHandler handler)
        {
            using (var discoveryClient = new DiscoveryClient(fixture.Configuration["JwtIssuer"], handler))
            {
                return await discoveryClient.GetAsync();
            }
        }
    }
}
