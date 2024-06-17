namespace MDLibrary.Domain.Entities
{
    public class LiteratureFile
    {
        public static string RootPath { get; set; }
        public int LiteratureFileId { get; set; }
        public Literature Literature { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
    }
}
