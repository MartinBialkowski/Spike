using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Spike.WebApi.Types.DTOs;

namespace Spike.WebApi.Mappings
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDTO, IdentityUser>();
        }
    }
}
