using MDLibrary.Models;
using System.Collections.Generic;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
	public class AuthorsIndexViewModel
	{
		public IEnumerable<AuthorsShortInfoViewModel> Authors { get; set;}
		public PagingInfo PagingInfo { get; set;}
	}
}
