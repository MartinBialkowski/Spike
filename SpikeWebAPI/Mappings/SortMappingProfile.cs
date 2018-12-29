using AutoMapper;
using Spike.Core.Entity;
using AutoSFaP.Models;
using AutoSFaP.Converters;

namespace Spike.WebApi.Mappings
{
    public class SortMappingProfile : Profile
    {
        public SortMappingProfile()
        {
            CreateMap<string, SortField<Student>[]>()
                .ConvertUsing(new SortFieldsConverter<Student>());
        }
    }
}
