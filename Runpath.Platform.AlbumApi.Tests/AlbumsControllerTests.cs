using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Runpath.Platform.AlbumApi.Controllers;
using Runpath.Platform.AlbumApi.Profiles;
using Runpath.Platform.AlbumApi.Responses;
using Runpath.Platform.AlbumApi.Services;
using Runpath.Platform.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Runpath.Platform.AlbumApi.Tests
{
    [TestClass]
    public class AlbumsControllerTests
    {
        readonly AlbumsController _albumController;
        readonly Mock<IAlbumService> _albumServiceMock;
        readonly Mock<ILogger<AlbumsController>> _loggerMock;

        public AlbumsControllerTests()
        {
            _albumServiceMock = new Mock<IAlbumService>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AlbumProfile());
                mc.AddProfile(new PhotoProfile());
            });
            IMapper _mapper = new Mapper(mapperConfig);

            _loggerMock = new Mock<ILogger<AlbumsController>>();

            _albumController = new AlbumsController(_albumServiceMock.Object, _mapper, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetAllAsync_HasNoAlbums_OKWithEmptyList()
        {
            var albums = new List<Album>();

            _albumServiceMock.Setup(x => x.GetAlbumsAsync())
                .ReturnsAsync(albums);

            var response = await _albumController.GetAllAsync();

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(IEnumerable<AlbumDetails>));
            Assert.IsFalse(response.Any());
        }

        [TestMethod]
        public async Task GetAllAsync_HasAlbums_OKWithAlbumList()
        {
            var albums = new List<Album>
            {
                new Album { Id = 1, Title = "One" },
                new Album { Id = 2, Title = "Two" },
                new Album { Id = 3, Title = "Three" },
            };

            _albumServiceMock.Setup(x => x.GetAlbumsAsync())
                .ReturnsAsync(albums);

            var response = await _albumController.GetAllAsync();

            Assert.AreEqual(3, response.Count());
        }

        [TestMethod]
        public async Task GetAllAsync_HasAlbumsAndPhotos_OKWithAlbumListIncludingPhotos()
        {
            var album1Photos = new List<Photo>
            {
                new Photo {Id = 1, Title = "The one photo",  AlbumId = 1},
                new Photo {Id = 2, Title = "The two photo", AlbumId = 1}
            };

            var album2Photos = new List<Photo>
            {
                new Photo {Id = 1, Title = "The second photo",  AlbumId = 2}
            };

            var album3Photos = new List<Photo>();

            var albums = new List<Album>
            {
                new Album { Id = 1, Title = "One", Photos = album1Photos },
                new Album { Id = 2, Title = "Two", Photos = album2Photos },
                new Album { Id = 3, Title = "Three", Photos = album3Photos },
            };

            _albumServiceMock.Setup(x => x.GetAlbumsAsync())
                .ReturnsAsync(albums);

            var response = await _albumController.GetAllAsync();

            var album1 = response.ElementAt(0);
            var album2 = response.ElementAt(1);
            var album3 = response.ElementAt(2);

            Assert.AreEqual(3, response.Count());
            Assert.IsInstanceOfType(album1.Photos, typeof(IEnumerable<PhotoDetails>));
            Assert.AreEqual(2, album1.Photos.Count());
            Assert.AreEqual(1, album2.Photos.Count());
            Assert.AreEqual(0, album3.Photos.Count());
        }

        [TestMethod]
        public async Task GetByIdAsync_AlbumDoesNotExist_NotFound()
        {
            _albumServiceMock.Setup(x => x.GetAlbumAsync(200))
               .ReturnsAsync(null as Album);

            var actionResult = await _albumController.GetByIdAsync(200);
            var notFoundResult = actionResult as NotFoundResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task GetByIdAsync_AlbumExist_OKWithAlbumIncludingPhotos()
        {
            var albumPhotos = new List<Photo>
            {
                new Photo {Id = 1, Title = "The one photo",  AlbumId = 1},
                new Photo {Id = 2, Title = "The two photo", AlbumId = 1}
            };

            var album = new Album { Id = 1, Title = "One", Photos = albumPhotos };

            _albumServiceMock.Setup(x => x.GetAlbumAsync(10))
               .ReturnsAsync(album);

            var actionResult = await _albumController.GetByIdAsync(10);
            var okResult = actionResult as OkObjectResult;
            var responseAlbum = okResult.Value as AlbumDetails;

            Assert.IsNotNull(okResult);
            Assert.IsNotNull(responseAlbum);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.IsInstanceOfType(responseAlbum.Photos, typeof(IEnumerable<PhotoDetails>));
            Assert.AreEqual(2, responseAlbum.Photos.Count());
        }

        [TestMethod]
        public async Task GetAlbumPhotosAsync_AlbumDoesNotExist_NotFound()
        {
            _albumServiceMock.Setup(x => x.GetAlbumPhotosAsync(200))
               .ReturnsAsync(null as IEnumerable<Photo>);

            var actionResult = await _albumController.GetAlbumPhotosAsync(200);
            var notFoundResult = actionResult as NotFoundResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task GetAlbumPhotosAsync_AlbumExist_OKWithPhotosList()
        {
            var albumPhotos = new List<Photo>
            {
                new Photo {Id = 1, Title = "The one photo",  AlbumId = 1},
                new Photo {Id = 2, Title = "The two photo", AlbumId = 1}
            };

            var album = new Album { Id = 1, Title = "One", Photos = albumPhotos };

            _albumServiceMock.Setup(x => x.GetAlbumAsync(10))
               .ReturnsAsync(album);

            _albumServiceMock.Setup(x => x.GetAlbumPhotosAsync(10))
               .ReturnsAsync(albumPhotos);

            var actionResult = await _albumController.GetAlbumPhotosAsync(10);
            var okResult = actionResult as OkObjectResult;
            var responsePhotos = okResult.Value as IEnumerable<PhotoDetails>;

            Assert.IsNotNull(okResult);
            Assert.IsNotNull(responsePhotos);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(2, responsePhotos.Count());
        }
    }
}
