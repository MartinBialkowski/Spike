using AutoMapper;

namespace SpikeWebAPI.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<StudentMappingProfile>();
                cfg.AddProfile<SortMappingProfile>();
                cfg.AddProfile<CourseMappingProfile>();
            });
        }
    }
}
