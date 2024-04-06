using System.ComponentModel.DataAnnotations;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
	public class AuthorsEditViewModel
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
	}
}
