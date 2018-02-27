using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EFCoreSpike5.Models;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spike.AuthenticationServer.IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // EF Core
            services.AddDbContext<EFCoreSpikeContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Identity Core
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<EFCoreSpikeContext>()
            .AddDefaultTokenProviders();

            services.AddIdentityServer(s => s.IssuerUri = Configuration["JwtIssuer"])
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryApiResources(GetResources())
                .AddInMemoryIdentityResources(GetIdentityResources())
                .AddInMemoryClients(GetClients())
                .AddAspNetIdentity<IdentityUser>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseMvc();
        }

        // Temporary in memory data, will remove at the end of research
        private IEnumerable<ApiResource> GetResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(Configuration["SpikeAudience"],
                "My API",
                claimTypes: new[] { "name", JwtRegisteredClaimNames.Email, ClaimTypes.Role, JwtRegisteredClaimNames.Birthdate })
            };
        }

        private IEnumerable<Client> GetClients()
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
                        new Secret(Configuration["SpikeSecret"].Sha256())
                    },
                    AllowedScopes = { Configuration["SpikeAudience"] }
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = Configuration["SpikeClientId"],
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AccessTokenLifetime = int.Parse(Configuration["JwtExpireSeconds"]),
                    ClientSecrets =
                    {
                        new Secret(Configuration["SpikeSecret"].Sha256())
                    },
                    AllowedScopes = { Configuration["SpikeAudience"] },
                    AllowOfflineAccess = true
                }
            };
        }

        public IEnumerable<IdentityResource> GetIdentityResources()
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
