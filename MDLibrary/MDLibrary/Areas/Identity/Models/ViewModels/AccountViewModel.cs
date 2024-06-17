using System.ComponentModel.DataAnnotations;

namespace MDLibrary.Areas.Identity.Models.ViewModels
{
	public class AccountViewModel
	{
		[Required(ErrorMessage = "Это поле обязательно")]
		[StringLength(100, ErrorMessage = "Имя пользователя должно иметь длину до {2} символов")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Это поле обязательно")]
		[EmailAddress]
		[StringLength(100, ErrorMessage = "E-mail должна иметь длину до {2} символов")]
		public string Email { get; set; }

		[Phone]
		public string? PhoneNumber { get; set; }
	}
}
