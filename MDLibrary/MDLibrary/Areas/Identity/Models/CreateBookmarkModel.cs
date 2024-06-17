namespace MDLibrary.Areas.Identity.Models
{
    public class CreateBookmarkModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public short? Page { get; set; }
        public int? LiteratureId { get; set; }
    }
}
