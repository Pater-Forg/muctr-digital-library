using System.ComponentModel.DataAnnotations;

namespace MDLibrary.Areas.Identity.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Это поле обязательно")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Это поле обязательно")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
