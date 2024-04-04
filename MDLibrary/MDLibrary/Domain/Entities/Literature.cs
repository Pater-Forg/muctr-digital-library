using System.Collections.Generic;
using System.Linq;

namespace MDLibrary.Domain.Entities
{
    public class Literature
    {
        public int LiteratureId { get; set; }
        public short? PublishYear { get; set; }
        public short? PageCount { get; set; }
        public string Caption { get; set; }
        public string? PublishLocation { get; set; }
        public string? Publisher { get; set; }
        public string? Isbn { get; set; }
        public string? Bbc { get; set; }
        public string? Udc { get; set; }
        public string? Abstract { get; set; }
        public ICollection<Keyword> Keywords { get; set; } = [];
        public ICollection<Author> Authors { get; set; } = [];
    }
}
