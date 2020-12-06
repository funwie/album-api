namespace Runpath.Platform.AlbumApi.Responses
{
    /// <summary>
    /// A Photo of an Album.
    /// </summary>
    public class PhotoDetails
    {
        /// <summary>
        /// The id of the photo.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The title of the photo.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Url to the photo.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The url to a thumbnail of the photo.
        /// </summary>
        public string ThumbnailUrl { get; set; }
    }
}
