using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Spike.AuthenticationServer.IdentityServer
{
	public class IdentityServerDbInitialize
    {
        private static PersistedGrantDbContext grantDbContext;
        private static ConfigurationDbContext configurationDbContext;
        public static void Initialize(IApplicationBuilder app, IConfiguration configuration)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                configurationDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                configurationDbContext.Database.Migrate();
                grantDbContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                grantDbContext.Database.Migrate();
                InitilizeDatabase(configuration);
            }
        }

        private static void InitilizeDatabase(IConfiguration configuration)
        {
            if (!configurationDbContext.Clients.Any())
            {
                foreach (var client in GetClients(configuration))
                {
                    configurationDbContext.Clients.Add(client.ToEntity());
                }
                configurationDbContext.SaveChanges();
            }

            if (!configurationDbContext.IdentityResources.Any())
            {
                foreach (var resource in GetIdentityResources())
                {
                    configurationDbContext.IdentityResources.Add(resource.ToEntity());
                }
                configurationDbContext.SaveChanges();
            }

            if (!configurationDbContext.ApiResources.Any())
            {
                foreach (var resource in GetApiResources(configuration))
                {
                    configurationDbContext.ApiResources.Add(resource.ToEntity());
                }
                configurationDbContext.SaveChanges();
            }
        }

        private static IEnumerable<ApiResource> GetApiResources(IConfiguration configuration)
        {
            return new List<ApiResource>
            {
                new ApiResource(configuration["SpikeAudience"],
                "My API",
                claimTypes: new[] { "name", JwtRegisteredClaimNames.Email, ClaimTypes.Role, JwtRegisteredClaimNames.Birthdate })
            };
        }

        private static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret(configuration["SpikeSecret"].Sha256())
                    },
                    AllowedScopes = { configuration["SpikeAudience"] }
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = configuration["SpikeClientId"],
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AccessTokenLifetime = int.Parse(configuration["JwtExpireSeconds"]),
                    ClientSecrets =
                    {
                        new Secret(configuration["SpikeSecret"].Sha256())
                    },
                    AllowedScopes = { configuration["SpikeAudience"] },
                    AllowOfflineAccess = true
                }
            };
        }

        private static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var customProfile = new IdentityResource(
                name: "custom.profile",
                displayName: "Custom profile",
                claimTypes: new[] { "name", "email" });

            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResources.Phone(),
                new IdentityResources.Address(),
                customProfile
            };
        }
    }
}
