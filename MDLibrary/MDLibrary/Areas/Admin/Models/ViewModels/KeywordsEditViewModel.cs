using System.ComponentModel.DataAnnotations;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
	public class KeywordsEditViewModel
	{
		public int Id { get; set; }
		[Required]
		[StringLength(64, ErrorMessage = "Максимальная длина слова 64 символа")]
		public string Value { get; set; }
	}
}
