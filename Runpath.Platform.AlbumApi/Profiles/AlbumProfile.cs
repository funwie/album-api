using AutoMapper;
using Runpath.Platform.AlbumApi.Responses;
using Runpath.Platform.Domain;

namespace Runpath.Platform.AlbumApi.Profiles
{
    public class AlbumProfile : Profile
    {
        public AlbumProfile()
        {
            CreateMap<Album, AlbumDetails>();
        }
    }
}
