using MDLibrary.Models;
using System.Collections.Generic;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
	public class KeywordsIndexViewModel
	{
		public IEnumerable<KeywordsShortInfoViewModel> Keywords { get; set; }
		public PagingInfo PagingInfo { get; set; }
	}
}
