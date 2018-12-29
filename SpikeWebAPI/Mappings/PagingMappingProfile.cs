using AutoMapper;
using AutoSFaP.Models;
using Spike.WebApi.Types.DTOs;

namespace Spike.WebApi.Mappings
{
    public class PagingMappingProfile : Profile
    {
        public PagingMappingProfile()
        {
            CreateMap<PagingDto, Paging>(MemberList.Source);
        }
    }
}
