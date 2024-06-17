using System.Collections.Generic;

namespace MDLibrary.Models.ViewModels
{
	public class LiteratureCardViewModel : LiteratureBaseViewModel
	{
		public int Id { get; set; }
		public bool HasFile { get; set; }
		public IEnumerable<string> KeywordsList { get; set; } = [];
	}
}
