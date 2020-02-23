using AutoMapper;
using AutoSFaP.Models;
using AutoSFaP.Converters;
using Spike.Core.Entity;
using Spike.WebApi.Types.DTOs;

namespace Spike.WebApi.Mappings
{
    public class FilterMappingProfile : Profile
    {
        public FilterMappingProfile()
        {
            CreateMap<StudentFilterDto, FilterField<Student>[]>()
                .ConvertUsing(s => FilterFieldsConverter<StudentFilterDto, Student>.Convert(s));
        }
    }
}
