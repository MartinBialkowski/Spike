using IdentityModel.Client;
using System.Threading.Tasks;
using Xunit;

namespace Spike.AuthenticationServer.IdentityServer.IntegrationTest
{
    public class AuthenticationTest : IClassFixture<AuthenticationFixture>
    {
        private AuthenticationFixture fixture;

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
            var secret = "secret";
            var username = "alice";
            var password = "password";
            var apiScope = "api1";
            var discovery = await DiscoveryClient.GetAsync("http://localhost:53702/");
            var tokenClient = new TokenClient(discovery.TokenEndpoint, clientId, secret);
            // act
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(username, password, apiScope);
            // assert
            Assert.False(tokenResponse.IsError);
            Assert.False(string.IsNullOrEmpty(tokenResponse.AccessToken));
        }
    }
}
