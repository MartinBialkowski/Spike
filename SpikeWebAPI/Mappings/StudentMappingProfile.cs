using AutoMapper;
using EFCoreSpike5.Models;
using Spike.Core.Entity;
using SpikeWebAPI.DTOs;

namespace Spike.WebApi.Mappings
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
