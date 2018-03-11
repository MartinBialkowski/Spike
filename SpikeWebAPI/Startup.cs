using Autofac;
using EFCoreSpike5.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Spike.WebApi.Modules;
using Spike.WebApi.Requirements;
using Swashbuckle.AspNetCore.Swagger;
using System.Net;
using System.Security.Claims;

namespace Spike.WebApi
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

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["JwtIssuer"];
                    options.RequireHttpsMetadata = false;

                    options.ApiName = Configuration["SpikeAudience"];
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Person", policy => policy.RequireClaim(ClaimTypes.Role));
                options.AddPolicy("Master", policy => policy.RequireClaim(ClaimTypes.Role, "Master"));
                options.AddPolicy("StudentDiscount", policy => policy.AddRequirements(new StudentDiscountRequirement()));
                options.AddPolicy("GetSelf", policy => policy.AddRequirements(new SelfRequirement()));
            });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Spike Controller", Version = "v1" });
            });

            // Enable Cors
            services.AddCors();

            services
                .AddMvc()
                .AddFluentValidation()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            var serviceProvider = services.BuildServiceProvider();
            SpikeDbInitializer.Initialize(serviceProvider);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule(Configuration["AutofacConfig:RepositoryConfig"]));
            builder.RegisterModule(new AutoMapperModule());
            builder.RegisterModule(new AuthorizationHandlerModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, EFCoreSpikeContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spike Controller v1");
            });

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseExceptionHandler(
              builder =>
              {
                  builder.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            //context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                        }
                    });
              });

            app.UseMvc();
        }
    }
}
