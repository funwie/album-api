using Runpath.Platform.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runpath.Platform.AlbumApi.Services
{
    public interface IAlbumService
    {
        Task<IEnumerable<Album>> GetAlbumsAsync();

        Task<Album> GetAlbumAsync(int albumId);

        Task<IEnumerable<Album>> GetUserAlbumsAsync(int userId);

        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserAsync(int userId);
        Task<IEnumerable<Photo>> GetAlbumPhotosAsync(int albumId);
    }
}
