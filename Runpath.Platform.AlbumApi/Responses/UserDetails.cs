using Runpath.Platform.Domain;
using System.Collections.Generic;

namespace Runpath.Platform.AlbumApi.Responses
{
    /// <summary>
    /// A User
    /// </summary>
    public class UserDetails
    {
        /// <summary>
        /// The id of the user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The username of the user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The address of the user
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// The mobile phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The website of the user
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// The company of the user
        /// </summary>
        public Company Company { get; set; }

        /// <summary>
        /// Albums owned by the user
        /// </summary>
        public IEnumerable<AlbumDetails> Albums { get; set; }
    }
}
