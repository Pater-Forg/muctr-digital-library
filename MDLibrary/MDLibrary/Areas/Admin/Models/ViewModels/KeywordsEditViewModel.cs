using System.ComponentModel.DataAnnotations;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
	public class KeywordsEditViewModel
	{
		public int Id { get; set; }
		[Required]
		public string Value { get; set; }
	}
}
