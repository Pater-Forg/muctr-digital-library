using MDLibrary.Models;
using System.Collections.Generic;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
	public class UsersIndexViewModel
	{
		public IEnumerable<UserViewModel> Users { get; set; }
		public PagingInfo PagingInfo { get; set; }
	}
}
