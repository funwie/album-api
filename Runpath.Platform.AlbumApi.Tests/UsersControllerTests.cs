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
    public class UsersControllerTests
    {
        readonly UsersController _userController;
        readonly Mock<IAlbumService> _albumServiceMock;
        readonly Mock<ILogger<UsersController>> _loggerMock;

        public UsersControllerTests()
        {
            _albumServiceMock = new Mock<IAlbumService>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new AlbumProfile());
                mc.AddProfile(new PhotoProfile());
            });
            IMapper _mapper = new Mapper(mapperConfig);
            _loggerMock = new Mock<ILogger<UsersController>>();

            _userController = new UsersController(_albumServiceMock.Object, _mapper, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetAllAsync_HasNoUsers_OKWithEmptyList()
        {
            var users = new List<User>();

            _albumServiceMock.Setup(x => x.GetUsersAsync())
                .ReturnsAsync(users);

            var response = await _userController.GetAllAsync();

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(IEnumerable<UserDetails>));
            Assert.IsFalse(response.Any());
        }

        [TestMethod]
        public async Task GetAllAsync_HasUsers_OKWithUserList()
        {
            var users = new List<User>
            {
                new User { Id = 1, Name = "One" },
                new User { Id = 2, Name = "Two" },
                new User { Id = 3, Name = "Three" },
            };

            _albumServiceMock.Setup(x => x.GetUsersAsync())
                .ReturnsAsync(users);

            var response = await _userController.GetAllAsync();

            Assert.AreEqual(3, response.Count());
        }


        [TestMethod]
        public async Task GetByIdAsync_UserDoesNotExist_NotFound()
        {
            _albumServiceMock.Setup(x => x.GetUserAsync(500))
               .ReturnsAsync(null as User);

            var actionResult = await _userController.GetByIdAsync(500);
            var notFoundResult = actionResult as NotFoundResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task GetByIdAsync_UserExist_OKWithUserIncludingAlbums()
        {
            var albums = new List<Album>
            {
                new Album {Id = 1, Title = "Album One",  UserId = 1},
                new Album {Id = 2, Title = "Album Two", UserId = 1}
            };

            var user = new User { Id = 1, Name = "One", Albums = albums };

            _albumServiceMock.Setup(x => x.GetUserAsync(5))
               .ReturnsAsync(user);

            var actionResult = await _userController.GetByIdAsync(5);
            var okResult = actionResult as OkObjectResult;
            var responseUser = okResult.Value as UserDetails;

            Assert.IsNotNull(okResult);
            Assert.IsNotNull(responseUser);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.IsInstanceOfType(responseUser.Albums, typeof(IEnumerable<AlbumDetails>));
            Assert.AreEqual(2, responseUser.Albums.Count());
        }

        [TestMethod]
        public async Task GetAlbumsAsync_UserDoesNotExist_NotFound()
        {
            _albumServiceMock.Setup(x => x.GetUserAlbumsAsync(500))
               .ReturnsAsync(null as IEnumerable<Album>);

            var actionResult = await _userController.GetAlbumsAsync(500);
            var notFoundResult = actionResult as NotFoundResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task GetAlbumsAsync_UserExist_OKWithAlbumsList()
        {
            var albums = new List<Album>
            {
                new Album {Id = 1, Title = "Album One",  UserId = 1},
                new Album {Id = 2, Title = "Album Two", UserId = 1}
            };

            var user = new User { Id = 1, Name = "One", Albums = albums };

            _albumServiceMock.Setup(x => x.GetUserAsync(10))
               .ReturnsAsync(user);

            _albumServiceMock.Setup(x => x.GetUserAlbumsAsync(10))
               .ReturnsAsync(albums);

            var actionResult = await _userController.GetAlbumsAsync(10);
            var okResult = actionResult as OkObjectResult;
            var responseAlbums = okResult.Value as IEnumerable<AlbumDetails>;

            Assert.IsNotNull(okResult);
            Assert.IsNotNull(responseAlbums);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(2, responseAlbums.Count());
        }
    }
}
