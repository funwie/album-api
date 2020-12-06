using AutoMapper;
using Runpath.Platform.AlbumApi.Responses;
using Runpath.Platform.Domain;

namespace Runpath.Platform.AlbumApi.Profiles
{
    public class PhotoProfile : Profile
    {
        public PhotoProfile()
        {
            CreateMap<Photo, PhotoDetails>();
        }
    }
}
