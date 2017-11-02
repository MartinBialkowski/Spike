using AutoMapper;
using EFCoreSpike5.ConstraintsModels;
using SpikeWebAPI.DTOs;

namespace SpikeWebAPI.Mappings
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
