using System.Collections.Generic;

namespace MDLibrary.Domain.Entities
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public ICollection<Literature> Literature { get; set; } = [];

        public override string ToString()
        {
            return Name;
        }
    }
}
