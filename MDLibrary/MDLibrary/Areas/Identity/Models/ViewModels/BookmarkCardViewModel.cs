namespace MDLibrary.Areas.Identity.Models.ViewModels
{
    public class BookmarkCardViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public short? PageNumber { get; set; }
        public int? LiteratureId { get; set; }
    }
}
