using System.ComponentModel.DataAnnotations;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
	public class LiteratureEditViewModel
	{
		public int Id { get; init; }
		public short? PublishYear { get; set; }
		public short? PageCount { get; set; }
		[Required]
		public string Caption { get; set; }
		public string? PublishLocation { get; set; }
		public string? Publisher { get; set; }
		public string? Isbn { get; set; }
		public string? Bbc { get; set; }
		public string? Udc { get; set; }
		public string? Abstract { get; set; }
		public string? Keywords { get; set; }
		public string? Authors { get; set; }
	}
}
