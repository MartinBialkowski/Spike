using AutoMapper;
using Spike.Core.Entity;
using Spike.WebApi.Types.DTOs;

namespace Spike.WebApi.Mappings
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            CreateMap<StudentCreateRequestDataTransferObject, Student>(MemberList.Source);
            CreateMap<StudentUpdateRequestDataTransferObject, Student>(MemberList.Source);
            CreateMap<Student, StudentResponseDataTransferObject>(MemberList.Destination);
        }
    }
}
