using System.Collections.Generic;

namespace MDLibrary.Models.ViewModels
{
	public class SearchViewModel
	{
		public SearchModel? SearchModel { get; set; }
		public IEnumerable<LiteratureCardViewModel>? SearchResults { get; set; }
		public PagingInfo? PagingInfo { get; set; }
	}
}
