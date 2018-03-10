using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Spike.AuthenticationServer.IdentityServer.Types.DTOs;

namespace Spike.AuthenticationServer.IdentityServer.Mappings
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDTO, IdentityUser>();
        }
    }
}
