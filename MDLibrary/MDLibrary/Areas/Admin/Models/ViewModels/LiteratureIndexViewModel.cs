using MDLibrary.Models;
using System.Collections.Generic;

namespace MDLibrary.Areas.Admin.Models.ViewModels
{
    public class LiteratureIndexViewModel
    {
        public IEnumerable<LiteratureIndexShortInfoViewModel> Literature { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
