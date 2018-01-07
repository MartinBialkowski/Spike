using AutoMapper;
using Spike.Core.Model;
using Spike.WebApi.Types.DTOs;

namespace Spike.WebApi.Mappings
{
    public class PagingMappingProfile : Profile
    {
        public PagingMappingProfile()
        {
            CreateMap<PagingDTO, Paging>()
                .ConvertUsing(new PagingConverter());
        }
    }

    public class PagingConverter : ITypeConverter<PagingDTO, Paging>
    {
        public Paging Convert(PagingDTO source, Paging destination, ResolutionContext context)
        {
            destination = new Paging
            {
                PageLimit = source.PageLimit.Value,
                PageNumber = source.PageNumber.Value
            };

            return destination;
        }
    }
}
