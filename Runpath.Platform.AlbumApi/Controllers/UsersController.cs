using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Runpath.Platform.AlbumApi.Responses;
using Runpath.Platform.AlbumApi.Services;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Runpath.Platform.AlbumApi.Controllers
{
    /// <summary>
    /// Users
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class UsersController : ControllerBase
    {
        readonly IAlbumService _albumService;
        readonly IMapper _mapper;
        readonly ILogger<UsersController> _logger;

        public UsersController(IAlbumService albumService, IMapper mapper, ILogger<UsersController> logger)
        {
            _albumService = albumService ?? throw new ArgumentNullException(nameof(albumService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get list of users
        /// </summary>
        /// <remarks>Returns empty list if no users exist</remarks>
        /// <response code="200">List of users</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDetails>), 200)]
        public async Task<IEnumerable<UserDetails>> GetAllAsync()
        {
            _logger.LogInformation("Getting User List");
            var users = await _albumService.GetUsersAsync();
            return _mapper.Map<IEnumerable<UserDetails>>(users);
        }

        /// <summary>
        /// Get a user along with their albums
        /// </summary>
        /// <param name="id">The user id</param>
        /// <response code="200">User and their albums</response>
        /// <response code="404">User not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDetails), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var user = await _albumService.GetUserAsync(id);
            if (user == null) return NotFound();
            return Ok(_mapper.Map<UserDetails>(user));
        }

        /// <summary>
        /// Get list of a user's albums
        /// </summary>
        /// <param name="id">The user id</param>
        /// <response code="200">List of albums</response>
        /// <response code="404">User not found</response>
        [HttpGet("{id}/albums")]
        [ProducesResponseType(typeof(IEnumerable<AlbumDetails>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAlbumsAsync(int id)
        {
            var user = await _albumService.GetUserAsync(id);
            if (user == null) return NotFound();

            var albums = await _albumService.GetUserAlbumsAsync(id);
            return Ok(_mapper.Map<IEnumerable<AlbumDetails>>(albums));
        }

    }
}
