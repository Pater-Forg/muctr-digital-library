using MDLibrary.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
    public class LiteratureAddViewModel : LiteratureBaseViewModel
	{
		public IFormFile? LiteratureFile { get; set; }
	}
}
