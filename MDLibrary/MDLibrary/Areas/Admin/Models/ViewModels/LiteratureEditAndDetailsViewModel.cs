using System.ComponentModel.DataAnnotations;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
	public class LiteratureEditAndDetailsViewModel
	{
		public int Id { get; init; }

		public short? PublishYear { get; set; }

		public short? PageCount { get; set; }

		[Required(ErrorMessage = "Это поле должно быть заполнено")]
		[StringLength(256, ErrorMessage = "Максимальная длина названия 256 символов")]
		public string? Caption { get; set; }

		[StringLength(64, ErrorMessage = "Максимальная длина места издания 64 символа")]
		public string? PublishLocation { get; set; }

		[StringLength(64, ErrorMessage = "Максимальная длина издательства 64 символа")]
		public string? Publisher { get; set; }

		[StringLength(32, ErrorMessage = "Максимальная длина ISBN 32 символа")]
		public string? Isbn { get; set; }

		[StringLength(32, ErrorMessage = "Максимальная длина ББК 32 символа")]
		public string? Bbc { get; set; }

		[StringLength(32, ErrorMessage = "Максимальная длина УДК 32 символа")]
		public string? Udc { get; set; }

		public string? Abstract { get; set; }

		public string? Keywords { get; set; }

		public string? Authors { get; set; }
	}
}
