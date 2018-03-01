using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
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
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddIdentityServer(s => s.IssuerUri = Configuration["JwtIssuer"])
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddAspNetIdentity<IdentityUser>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(Configuration.GetConnectionString("IdentityServerConfigureConnection"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(Configuration.GetConnectionString("IdentityServerGrantsConnection"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = int.Parse(Configuration["TokenCleanupSeconds"]);
                });

            services.AddMvc();

            var serviceProvider = services.BuildServiceProvider();
            IdentityServerDbInitialize.Initialize(serviceProvider, Configuration);
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
    }
}
