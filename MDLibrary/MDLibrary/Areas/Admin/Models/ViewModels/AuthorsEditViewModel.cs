using System.ComponentModel.DataAnnotations;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
	public class AuthorsEditViewModel
	{
		public int Id { get; set; }

		[Required]
		[StringLength(32, ErrorMessage = "Максимальная длина имени 32 символа")]
		public string Name { get; set; }
	}
}
