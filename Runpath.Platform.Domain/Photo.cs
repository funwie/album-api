using System;

namespace Runpath.Platform.Domain
{
    public class Photo : Entity
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
