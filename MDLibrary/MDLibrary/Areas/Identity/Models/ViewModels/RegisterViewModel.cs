using System.ComponentModel.DataAnnotations;

namespace MDLibrary.Areas.Identity.Models.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Это поле обязательно")]
		[EmailAddress]
		[StringLength(100, ErrorMessage = "E-mail должна иметь длину до {2} символов")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Это поле обязательно")]
		[StringLength(100, ErrorMessage = "Имя пользователя должно иметь длину до {2} символов")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Это поле обязательно")]
		[StringLength(100, ErrorMessage = "Пароль должен иметь длину от {2} до символов {1}, а также обязательно содержать заглавные и строчные буквы и цифры", MinimumLength = 8)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Пароли должны совпадать")]
		public string ConfirmPassword { get; set; }
	}
}
