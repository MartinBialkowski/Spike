using AutoMapper;
using Spike.Core.Entity;
using Spike.WebApi.Types.DTOs;
using System;
using Xunit;

namespace Spike.WebApi.Mappings.Test
{
    public class StudentMappingTest: IDisposable
    {
        [Fact]
        public void ShouldMapConfigurationBeValid()
        {
            Mapper.Initialize(m => m.AddProfile<StudentMappingProfile>());
            Mapper.AssertConfigurationIsValid();
        }

        [Fact]
        public void ShouldMapStudentCreateDtoToStudent()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<StudentCreateRequestDataTransferObject, Student>(MemberList.Source));
            Mapper.AssertConfigurationIsValid();
        }

        [Fact]
        public void ShouldMapStudentUpdateDtoToStudent()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<StudentUpdateRequestDataTransferObject, Student>(MemberList.Source));
            Mapper.AssertConfigurationIsValid();
        }

        [Fact]
        public void ShouldMapStudentToStudentResponse()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Student, StudentResponseDataTransferObject>(MemberList.Destination));
            Mapper.AssertConfigurationIsValid();
        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
