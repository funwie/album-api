using AutoMapper;
using Runpath.Platform.AlbumApi.Responses;
using Runpath.Platform.Domain;

namespace Runpath.Platform.AlbumApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDetails>();
        }
    }
}
