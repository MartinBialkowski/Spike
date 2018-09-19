using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Spike.AuthenticationServer.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, builder) =>
                {
                    var configuration = hostingContext.Configuration.GetSection("Logging");
                    builder.AddFile(configuration);
                })
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>();
    }
}
