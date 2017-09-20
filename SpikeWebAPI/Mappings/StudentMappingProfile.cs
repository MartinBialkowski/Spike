using AutoMapper;
using EFCoreSpike5.Models;
using SpikeWebAPI.DTOs;

namespace SpikeWebAPI.Mappings
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            CreateMap<StudentCreateRequestDataTransferObject, Student>();
            CreateMap<StudentUpdateRequestDataTransferObject, Student>();
            CreateMap<Student, StudentResponseDataTransferObject>();
        }
    }
}
