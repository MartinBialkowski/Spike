using Autofac;
using Autofac.Configuration;
using AutoSFaP;
using Microsoft.Extensions.Configuration;
using Spike.Core.Entity;
using System.IO;

namespace Spike.WebApi.Modules
{
    public class RepositoryModule : Module
    {
        private readonly string configPath;

        public RepositoryModule(string configPath)
        {
            this.configPath = configPath;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var autofacConfig = new ConfigurationBuilder();
            autofacConfig.SetBasePath(Directory.GetCurrentDirectory());
            autofacConfig.AddJsonFile(configPath);
            var repositoryModule = new ConfigurationModule(autofacConfig.Build());

            builder.RegisterModule(repositoryModule);
            builder.RegisterType<DataLimiter<Student>>().As<IDataLimiter<Student>>();
        }
    }
}
