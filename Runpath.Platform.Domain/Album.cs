using System.Collections.Generic;

namespace Runpath.Platform.Domain
{
    public class Album : Entity
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
    }
}
