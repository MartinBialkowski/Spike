using AutoMapper;
using AutoSFaP.Models;
using Spike.WebApi.Types.DTOs;

namespace Spike.WebApi.Mappings
{
    public class PagingMappingProfile : Profile
    {
        public PagingMappingProfile()
        {
            CreateMap<PagingDto, Paging>()
                .ConvertUsing(new PagingConverter());
        }
    }

    public class PagingConverter : ITypeConverter<PagingDto, Paging>
    {
        public Paging Convert(PagingDto source, Paging destination, ResolutionContext context)
        {
            var result = new Paging
            {
                PageLimit = source.PageLimit.Value,
                PageNumber = source.PageNumber.Value
            };

            return result;
        }
    }
}
