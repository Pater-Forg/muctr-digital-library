using System.Collections.Generic;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
	public class AuthorsDetailsViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public IDictionary<int, string> Literature { get; set; }
	}
}
