using System.Collections.Generic;

namespace Runpath.Platform.AlbumApi.Responses
{
    /// <summary>
    /// An Album 
    /// </summary>
    public class AlbumDetails
    {
        /// <summary>
        /// Id of the album.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// If of the user that owns the album.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Title of the album
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Photos of the album
        /// </summary>
        public IEnumerable<PhotoDetails> Photos { get; set; }
    }
}
