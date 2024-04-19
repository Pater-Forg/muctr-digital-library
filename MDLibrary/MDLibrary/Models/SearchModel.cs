namespace MDLibrary.Models
{
	public class SearchModel
	{
		public string? SearchQuery { get; set; }
		public string? Authors { get; set; }
		public string? Keywords { get; set; }
		public int? PublishYearLower { get; set; }
		public int? PublishYearUpper { get; set; }
	}
}
