using MDLibrary.Areas.Admin.Models.ViewModels;
using MDLibrary.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;

namespace MDLibrary.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LiteratureController : Controller
    {
        private readonly MDLibraryDbContext _context;

        public LiteratureController(MDLibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            [FromQuery(Name = "f")] string? filter,
            [FromQuery(Name = "p")] int page = 1,
            [FromQuery(Name = "n")] int itemsPerPage = 20
            )
        {
            var literatureEntities = _context.Literature
                .Include(x => x.Authors)
                .Select(x => x);

            if (!filter.IsNullOrEmpty())
            {
                ViewData["Filter"] = filter;
                int.TryParse(filter, out var id);
                literatureEntities = from x in literatureEntities
                                     where
                                        (id != 0 && x.LiteratureId == id) ||
                                        EF.Functions.Like(x.Caption, $"%{filter}%") ||
                                        x.Authors.Any(a => EF.Functions.Like(a.Name, $"%{filter}%"))
                                     select x;
            }
            var itemsCount = literatureEntities.Count();
            var literatureViewModels = await literatureEntities
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Select(x => new LiteratureViewModel
                {
                    Id = x.LiteratureId,
                    Caption = x.Caption,
                    Authors = string.Join(", ", x.Authors)
                })
                .AsNoTracking()
                .ToListAsync();
            return View(new LiteratureIndexViewModel
            {
                Literature = literatureViewModels,
                PagingInfo = new MDLibrary.Models.PagingInfo
                {
                    ItemsPerPage = itemsPerPage,
                    TotalItems = itemsCount,
                    CurrentPage = page
                }
            });
        }
    }
}
