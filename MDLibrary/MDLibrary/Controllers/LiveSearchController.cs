using MDLibrary.Domain;
using MDLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace MDLibrary.Controllers
{
	public class LiveSearchController : Controller
	{
		private readonly MDLibraryBusinessDbContext _context;
        public LiveSearchController(MDLibraryBusinessDbContext context)
        {
			_context = context;
        }

		[HttpPost]
        public IActionResult Authors(string query)
		{
			if (query.IsNullOrEmpty())
				return PartialView(Enumerable.Empty<AuthorLiveSearch>());
			query = query.Split(", ").Last();
			if (query.IsNullOrEmpty())
				return PartialView(Enumerable.Empty<AuthorLiveSearch>());
			var authors = _context.Authors
				.Where(a => EF.Functions.ILike(a.Name, $"{query}%"))
				.Select(a => new AuthorLiveSearch
				{
					Id = a.AuthorId,
					Name = a.Name
				}).AsEnumerable();

			return PartialView(authors);
		}
	}
}
