using Autofac;
using AutoMapper;
using Spike.WebApi.Mappings;

namespace Spike.WebApi.Modules
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StudentMappingProfile>();
                cfg.AddProfile<SortMappingProfile>();
                cfg.AddProfile<CourseMappingProfile>();
                cfg.AddProfile<FilterMappingProfile>();
                cfg.AddProfile<PagingMappingProfile>();
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }
}
