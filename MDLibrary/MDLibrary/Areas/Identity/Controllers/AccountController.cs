using MDLibrary.Areas.Identity.Models.ViewModels;
using MDLibrary.Domain;
using MDLibrary.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MDLibrary.Areas.Identity.Controllers
{
	[Area("Identity")]
	[Authorize]
	public class AccountController : Controller
	{
		private readonly UserManager<LibraryUser> _userManager;
		private readonly MDLibraryBusinessDbContext _context;

		public short BookmarksPerPage = 5;

        public AccountController(UserManager<LibraryUser> userManager, MDLibraryBusinessDbContext context)
        {
            _userManager = userManager;
			_context = context;
        }

        public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return RedirectToAction("Login", "Auth");
			}
			return View(new AccountViewModel
			{
				UserName = user.UserName!,
				Email = user.Email!,
				PhoneNumber = user.PhoneNumber
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(AccountViewModel accountModel)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var user = await _userManager.GetUserAsync(User);
			if (user is not null)
			{
				user.UserName = accountModel.UserName;
				user.PhoneNumber = accountModel.PhoneNumber;
				user.Email = accountModel.Email;
				var result = await _userManager.UpdateAsync(user);
				ViewData["Status"] = result.Succeeded;
				return View();
			}

			ModelState.AddModelError("", "Что-то пошло не так");
			return View();
		}

		public IActionResult Bookmarks([FromQuery(Name = "p")] short page = 1)
		{
			var userId = _userManager.GetUserId(User);
			var bookmarks = _context.Bookmarks
				.Include(b => b.LiteraturePage)
				.ThenInclude(p => p.Literature)
				.Where(b => b.UserId == userId)
				.Skip((page - 1) * BookmarksPerPage)
				.Take(BookmarksPerPage)
				.AsNoTracking()
				.Select(b => new BookmarkCardViewModel
				{
					Id = b.Id,
					Title = b.Title,
					Description = b.Description,
					PageNumber = b.LiteraturePage == null
						? null
						: b.LiteraturePage.PageNumber,
					LiteratureId = b.LiteraturePage == null
						? null
						: b.LiteraturePage.Literature.LiteratureId,
					LiteratureTitle = b.LiteraturePage == null
                        ? null
                        : b.LiteraturePage.Literature.Caption,

                })
				.AsEnumerable();
			return View(new BookmarksViewModel
			{
				Bookmarks = bookmarks,
				PagingInfo = new MDLibrary.Models.PagingInfo
				{
					CurrentPage = page,
					ItemsPerPage = BookmarksPerPage,
					TotalItems = _context.Bookmarks.Count()
				}
			});
		}

		public async Task<IActionResult> GetBookmarksPageList([FromQuery] int? literatureId)
		{
			if (literatureId == null)
			{
				return new StatusCodeResult(StatusCodes.Status400BadRequest);
			}
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return new StatusCodeResult(StatusCodes.Status401Unauthorized);
			}
			var bookmarksPageList = _context.Bookmarks
				.Where(b => b.UserId == user.Id)
				.Include(b => b.LiteraturePage)
				.Select(b => b.LiteraturePage.PageNumber)
				.ToList();
			return Json(new {bookmarksPageList = bookmarksPageList});
		}

		public async Task<IActionResult> CreateOrDeleteBookmark([FromQuery] int? literatureId, [FromQuery] short? page)
		{
			if (literatureId == null || page == null)
			{
				return new StatusCodeResult(StatusCodes.Status400BadRequest);
			}

			var literaturePage = await _context.LiteraturePages
				.Include(p => p.Literature)
				.FirstOrDefaultAsync(p => p.Literature.LiteratureId == literatureId && p.PageNumber == page);

			if (literaturePage == null)
			{
				return NotFound();
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return new StatusCodeResult(StatusCodes.Status401Unauthorized);
			}

			// try to receive bookmark from db
			var bookmark = await _context.Bookmarks
				.FirstOrDefaultAsync(b => b.UserId == user.Id && b.LiteraturePage == literaturePage);

			// if bookmark is already exist then delete it
			if (bookmark != null)
			{
				_context.Bookmarks.Remove(bookmark);
				try
				{
					_context.SaveChanges();
                    return Ok();
                }
				catch
				{
					return new StatusCodeResult(StatusCodes.Status500InternalServerError);
				}
			}

			// else add new bookmark

			bookmark = new Bookmark
			{
				UserId = user.Id,
				LiteraturePage = literaturePage,
				Title = null,
				Description = null
			};

			_context.Bookmarks.Add(bookmark);
			try
			{
				_context.SaveChanges();
                return Ok();
            }
			catch
			{
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
