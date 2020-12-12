using Microsoft.Extensions.Logging;
using Runpath.Platform.AlbumApi.Serializers;
using Runpath.Platform.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            var album = await _serializer.DeserializeJsonAsync<Album>(albumJsonData);

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
            var users = await _serializer.DeserializeJsonAsync<IEnumerable<User>>(usersJsonData);
            return users;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            var userPath = $"{USERS}/{userId}";

            _logger.LogInformation("Get User {Id} from {Url}", userId, userPath);

            var userJsonData = await GetJsonAsync(userPath);
            var user = await _serializer.DeserializeJsonAsync<User>(userJsonData);

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
            var albums = await _serializer.DeserializeJsonAsync<IEnumerable<Album>>(albumsJsonData);

            if (albums == null || !albums.Any()) return albums;

            await Task.WhenAll(albums.Select(GetPhotosAsync));
            return albums;
        }

        private async Task GetPhotosAsync(Album album)
        {
            album.Photos = await GetPhotosAsync(album.Id);
        }

        private async Task<IEnumerable<Photo>> GetPhotosAsync(int albumId)
        {
            var photosPath = $"{ALBUMS}/{albumId}/{PHOTOS}";

            _logger.LogInformation("Get photos for album {Id} from {Url}", albumId, photosPath);

            var photosJsonData = await GetJsonAsync(photosPath);
            var photos = await _serializer.DeserializeJsonAsync<IEnumerable<Photo>>(photosJsonData);
            return photos;
        }

        private async Task<string> GetJsonAsync(string resoursePath)
        {
            if (string.IsNullOrWhiteSpace(resoursePath)) throw new ArgumentNullException(nameof(resoursePath));

            try
            {
                _logger.LogInformation("Requesting data from url {BaseAddress}{Url}", _httpClient.BaseAddress, resoursePath);
                var response = await _httpClient.GetAsync(resoursePath);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content ?? string.Empty;
                }

                _logger.LogInformation("Request {BaseAddress}{Url} was unsuccessful with Code", _httpClient.BaseAddress, resoursePath, response.StatusCode);
                throw new Exception($"Unsuccessful request to url { _httpClient.BaseAddress}{resoursePath}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogDebug("Failed to get from {Url}", resoursePath);
                _logger.LogError("HttpRequestException {Message}", ex.Message);
                throw ex;
            }
        }
    }
}
