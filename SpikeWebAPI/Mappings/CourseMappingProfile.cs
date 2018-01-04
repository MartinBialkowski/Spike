using AutoMapper;
using Spike.Core.Entity;
using SpikeWebAPI.DTOs;

namespace Spike.WebApi.Mappings
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<Course, CourseResponseDataTransferObject>();
        }
    }
}