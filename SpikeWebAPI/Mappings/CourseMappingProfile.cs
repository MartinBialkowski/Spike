using AutoMapper;
using EFCoreSpike5.Models;
using SpikeWebAPI.DTOs;

namespace SpikeWebAPI.Mappings
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<Course, CourseResponseDataTransferObject>();
        }
    }
}