using Microsoft.Extensions.Logging;
using Runpath.Platform.AlbumApi.Serializers;
using Runpath.Platform.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Runpath.Platform.AlbumApi.Services
{
    /// <summary>
    /// Client to access https://jsonplaceholder.typicode.com/
    /// </summary>
    public class AlbumService : IAlbumService
    {
        readonly string ALBUMS = "albums";
        readonly string PHOTOS = "photos";
        readonly string USERS = "users";

        readonly HttpClient _httpClient;
        readonly ILogger<AlbumService> _logger;
        private readonly ISerializer _serializer;

        public AlbumService(HttpClient httpClient, 
                            ILogger<AlbumService> logger,
                            ISerializer serializer)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serializer = serializer;
        }

        public async Task<IEnumerable<Album>> GetAlbumsAsync()
        {
            _logger.LogInformation("Get Albums");
            return await GetAlbumsAsync(ALBUMS);
        }

        public async Task<Album> GetAlbumAsync(int albumId)
        {
            var albumPath = $"{ALBUMS}/{albumId}";

            _logger.LogInformation("Get album {Id} from {Url}", albumId, albumPath);
            var albumJsonData = await GetJsonAsync(albumPath);
            var album = _serializer.DeserializeJson<Album>(albumJsonData);

            if (album == null) return album;

            album.Photos = await GetPhotosAsync(albumId);

            return album;
        }

        public async Task<IEnumerable<Photo>> GetAlbumPhotosAsync(int albumId)
        {
            return await GetPhotosAsync(albumId);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var usersJsonData = await GetJsonAsync(USERS);
            var users = _serializer.DeserializeJson<IEnumerable<User>>(usersJsonData);
            return users;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            var userPath = $"{USERS}/{userId}";

            _logger.LogInformation("Get User {Id} from {Url}", userId, userPath);

            var userJsonData = await GetJsonAsync(userPath);
            var user = _serializer.DeserializeJson<User>(userJsonData);

            if (user == null) return user;

            user.Albums = await GetUserAlbumsAsync(userId);

            return user;
        }

        public async Task<IEnumerable<Album>> GetUserAlbumsAsync(int userId)
        {
            var userAlbumsPath = $"{USERS}/{userId}/{ALBUMS}";

            _logger.LogInformation("Get albums for user {Id} from {Url}", userId, userAlbumsPath);

            return await GetAlbumsAsync(userAlbumsPath);
        }

        private async Task<IEnumerable<Album>> GetAlbumsAsync(string path)
        {
            var albumsJsonData = await GetJsonAsync(path);
            var albums = _serializer.DeserializeJson<IEnumerable<Album>>(albumsJsonData);
            foreach (var album in albums)
            {
                album.Photos = await GetPhotosAsync(album.Id);
            }
            return albums;
        }

        private async Task<IEnumerable<Photo>> GetPhotosAsync(int albumId)
        {
            var photosPath = $"{ALBUMS}/{albumId}/{PHOTOS}";

            _logger.LogInformation("Get photos for album {Id} from {Url}", albumId, photosPath);

            var photosJsonData = await GetJsonAsync(photosPath);
            var photos = _serializer.DeserializeJson<IEnumerable<Photo>>(photosJsonData);
            return photos;
        }

        private async Task<string> GetJsonAsync(string resoursePath)
        {
            if (string.IsNullOrWhiteSpace(resoursePath)) throw new ArgumentNullException(nameof(resoursePath));

            try
            {
                _logger.LogInformation("Requesting data from url {Url}", resoursePath);
                return await _httpClient.GetStringAsync(resoursePath);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogDebug("Failed to get from {Url}", resoursePath);
                _logger.LogError("HttpRequestException {Message}", ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get from {Message}", ex.Message);
                throw ex;
            }
        }
    }
}
