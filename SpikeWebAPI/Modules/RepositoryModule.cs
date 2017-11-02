using Autofac;
using SpikeRepo.Abstract;
using SpikeRepo.Repositories;

namespace SpikeWebAPI.Modules
{
    // Left just for learning purpose
    public class RepositoryModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StudentRepository>().As<IStudentRepository>();
            builder.RegisterType<CourseRepository>().As<ICourseRepository>();
        }
    }
}
