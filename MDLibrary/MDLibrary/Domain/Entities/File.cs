using System.ComponentModel.DataAnnotations.Schema;

namespace MDLibrary.Domain.Entities
{
    public class File
    {
        public int FileId { get; set; }
        public Literature Literature { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public byte[] Content { get; set; }
    }
}
