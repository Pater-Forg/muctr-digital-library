using MDLibrary.Domain;
using MDLibrary.Helpers;
using MDLibrary.Models;
using MDLibrary.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MDLibrary.Controllers
{
    public class SearchController : Controller
	{
		private readonly MDLibraryBusinessDbContext _context;
		public int ItemsPerPage { get; set; } = 10;

        public SearchController(MDLibraryBusinessDbContext context)
        {
			_context = context;
        }
        public IActionResult Index(SearchViewModel searchViewModel, int? page)
		{
			if (searchViewModel.SearchModel is not null)
			{
				page ??= 1;
				var searchResults = _context.Literature.Search(searchViewModel.SearchModel);

				searchViewModel.PagingInfo = new PagingInfo
				{
					CurrentPage = page.Value,
					ItemsPerPage = this.ItemsPerPage,
					TotalItems = searchResults.Count()
				};

				searchViewModel.SearchResults = searchResults
					.Skip((page.Value - 1) * ItemsPerPage)
					.Take(ItemsPerPage)
					.Select(literature => new LiteratureCardViewModel
					{
						Id = literature.LiteratureId,
						Caption = literature.Caption,
						PublishYear = literature.PublishYear,
						PageCount = literature.PageCount,
						PublishLocation = literature.PublishLocation,
						Publisher = literature.Publisher,
						Isbn = literature.Isbn,
						Bbc = literature.Bbc,
						Udc = literature.Udc,
						Abstract = literature.Abstract,
						Keywords = literature.Keywords == null
									? null
									: string.Join(", ", literature.Keywords),
						Authors = literature.Authors == null
									? null
									: string.Join(", ", literature.Authors),
						HasFile = _context.LiteratureFiles
										  .Include(f => f.Literature)
										  .Any(f => f.Literature.LiteratureId == literature.LiteratureId)
					});
			}

			return View(searchViewModel);
		}
	}
}
