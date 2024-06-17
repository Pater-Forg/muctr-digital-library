namespace MDLibrary.Domain.Entities
{
	public class LiteraturePage
	{
		public int LiteraturePageId { get; set; }
		public Literature Literature { get; set; }
		public short PageNumber { get; set; }
		public string Text { get; set; }
	}
}
