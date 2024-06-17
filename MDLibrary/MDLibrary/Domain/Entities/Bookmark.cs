namespace MDLibrary.Domain.Entities
{
    public class Bookmark
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public LiteraturePage? LiteraturePage { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

    }
}
