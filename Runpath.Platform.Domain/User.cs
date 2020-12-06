using System.Collections.Generic;

namespace Runpath.Platform.Domain
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public Company Company { get; set; }
        public IEnumerable<Album> Albums { get; set; }
    }
}
