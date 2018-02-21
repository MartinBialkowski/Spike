using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Spike.AuthenticationServer.IdentityServer.IntegrationTest
{
    public class AuthenticationTest : IClassFixture<AuthenticationFixture>
    {
        private AuthenticationFixture fixture;
        private string secret = "secret";
        private string apiScope = "api1";
        public AuthenticationTest(AuthenticationFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldAuthenticateWhenUsingResourceOwnerPassword()
        {
            // arrange
            var clientId = "ro.client";
            var username = "alice";
            var password = "password";
            DiscoveryResponse discovery;
            TokenResponse tokenResponse;
            var handler = fixture.server.CreateHandler();
            // act
            using (var discoveryClient = new DiscoveryClient(fixture.server.BaseAddress.AbsoluteUri, handler))
            {
                discovery = await discoveryClient.GetAsync();
            }
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
            var clientId = "client";
            DiscoveryResponse discovery;
            TokenResponse tokenResponse;
            var handler = fixture.server.CreateHandler();
            // act
            using (var discoveryClient = new DiscoveryClient(fixture.server.BaseAddress.AbsoluteUri, handler))
            {
                discovery = await discoveryClient.GetAsync();
            }
            using (var tokenClient = new TokenClient(discovery.TokenEndpoint, clientId, secret, handler))
            {
                tokenResponse = await tokenClient.RequestClientCredentialsAsync(apiScope);
            }
            // assert
            Assert.False(tokenResponse.IsError);
            Assert.False(string.IsNullOrEmpty(tokenResponse.AccessToken));
        }
    }
}
