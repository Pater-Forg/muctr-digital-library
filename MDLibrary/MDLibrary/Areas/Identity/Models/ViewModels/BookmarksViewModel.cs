using MDLibrary.Models;
using System.Collections.Generic;

namespace MDLibrary.Areas.Identity.Models.ViewModels
{
    public class BookmarksViewModel
    {
        public IEnumerable<BookmarkCardViewModel> Bookmarks { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
