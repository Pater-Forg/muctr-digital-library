using MDLibrary.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
    public class LiteratureEditViewModel : LiteratureBaseViewModel
	{
		public int Id { get; set; }
		public IFormFile? LiteratureFile { get; set; }
		public bool HasFile { get; set; }
	}
}
