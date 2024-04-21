namespace MDLibrary.Models
{
	public class SearchModel
	{
		public string? SearchQuery { get; set; }
		public string? Authors { get; set; }
		public int? PublishYearLower { get; set; }
		public int? PublishYearUpper { get; set; }
		public bool SearchInAbstract { get; set; }
		public bool SearchInKeywords { get; set; }
		public bool SearchInText { get; set; }
		public bool SearchInCaption { get; set; }
	}
}
