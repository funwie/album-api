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
    /// Albums
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class AlbumsController : ControllerBase
    {
        readonly IAlbumService _albumService;
        readonly IMapper _mapper;
        readonly ILogger<AlbumsController> _logger;

        public AlbumsController(IAlbumService albumService, IMapper mapper, ILogger<AlbumsController> logger)
        {
            _albumService = albumService ?? throw new ArgumentNullException(nameof(albumService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get list of albums along with their photos
        /// </summary>
        /// <remarks>Returns empty list if no alums exist</remarks>
        /// <response code="200">List of albums along with their photos or empty list</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AlbumDetails>), 200)]
        public async Task<IEnumerable<AlbumDetails>> GetAllAsync()
        {
            _logger.LogInformation("Getting Album List");
            var albums = await _albumService.GetAlbumsAsync();
            return _mapper.Map<IEnumerable<AlbumDetails>>(albums);
        }

        /// <summary>
        /// Get an album along with its photos
        /// </summary>
        /// <param name="id">The album id</param>
        /// <response code="200">Album and its photos</response>
        /// <response code="404">Album not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AlbumDetails), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            _logger.LogInformation("Getting Album with id {Id}", id);
            var album = await _albumService.GetAlbumAsync(id);
            if (album == null) return NotFound();
            return Ok(_mapper.Map<AlbumDetails>(album));
        }

        /// <summary>
        /// Get list of photos that belong to an album
        /// </summary>
        /// <param name="id">The album id</param>
        /// <response code="200">List of photos</response>
        /// <response code="404">Album not found</response>
        [HttpGet("{id}/photos")]
        [ProducesResponseType(typeof(IEnumerable<PhotoDetails>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAlbumPhotosAsync(int id)
        {
            _logger.LogInformation("Getting Album with id {}", id);
            var album = await _albumService.GetAlbumAsync(id);
            if (album == null) return NotFound();

            _logger.LogInformation("Getting Photos for album with id {}", id);
            var photos = await _albumService.GetAlbumPhotosAsync(id);
            return Ok(_mapper.Map<IEnumerable<PhotoDetails>>(photos));
        }
    }
}
