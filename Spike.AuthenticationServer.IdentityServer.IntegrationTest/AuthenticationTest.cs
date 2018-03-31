using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Spike.AuthenticationServer.IdentityServer.IntegrationTest
{
    public class AuthenticationTest : IClassFixture<AuthenticationFixture>
    {
        private readonly AuthenticationFixture fixture;
        private readonly string secret;
        private readonly string apiScope;
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
            var handler = fixture.Server.CreateHandler();
            var discovery = await GetDiscoveryResponse(handler);
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
            const string clientId = "client";
            var handler = fixture.Server.CreateHandler();
            var discovery = await GetDiscoveryResponse(handler);
            // act
            using (var tokenClient = new TokenClient(discovery.TokenEndpoint, clientId, secret, handler))
            {
                tokenResponse = await tokenClient.RequestClientCredentialsAsync(apiScope);
            }
            // assert
            Assert.False(tokenResponse.IsError);
            Assert.False(string.IsNullOrEmpty(tokenResponse.AccessToken));
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldAuthenticateWhenUsingResourceOwnerFlowReferenceToken()
        {
            // arrange
            TokenResponse tokenResponse;
            IntrospectionResponse introspectionResponse;
            var referenceScopeName = fixture.Configuration["SpikeReferenceAudience"];
            var clientId = fixture.Configuration["SpikeReferenceClient"];
            var username = fixture.Configuration["SpikeTestUsername"];
            var password = fixture.Configuration["SpikeTestPassword"];
            var scopeSecret = fixture.Configuration["ScopeReferenceSecret"];
            var handler = fixture.Server.CreateHandler();
            DiscoveryResponse discovery = await GetDiscoveryResponse(handler);
            // act
            using (var tokenClient = new TokenClient(discovery.TokenEndpoint, clientId, secret, handler))
            {
                tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(username, password, apiScope);
            }
            using (var introspectionClient = new IntrospectionClient(discovery.IntrospectionEndpoint, referenceScopeName, scopeSecret, handler))
            {
                introspectionResponse = await introspectionClient.SendAsync(new IntrospectionRequest() { Token = tokenResponse.AccessToken });
            }
            // assert
            Assert.False(introspectionResponse.IsError);
            Assert.False(string.IsNullOrEmpty(introspectionResponse.Raw));
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
