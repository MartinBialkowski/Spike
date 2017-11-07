using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SpikeWebAPI.Modules
{
    public class RepositoryModule: Module
    {
        private string configAbsolutePath;

        public RepositoryModule(string configPath)
        {
            configAbsolutePath = Path.GetFullPath(configPath);
        }

        protected override void Load(ContainerBuilder builder)
        {
            var autofacConfig = new ConfigurationBuilder();
            autofacConfig.AddJsonFile(configAbsolutePath);
            var repositoryModule = new ConfigurationModule(autofacConfig.Build());

            builder.RegisterModule(repositoryModule);
        }
    }
}
