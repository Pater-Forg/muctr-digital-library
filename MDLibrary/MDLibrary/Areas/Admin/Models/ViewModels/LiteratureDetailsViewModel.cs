using MDLibrary.Models.ViewModels;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
    public class LiteratureDetailsViewModel : LiteratureBaseViewModel
	{
		public int Id { get; init; }
		public bool HasFile { get; set; }
	}
}
