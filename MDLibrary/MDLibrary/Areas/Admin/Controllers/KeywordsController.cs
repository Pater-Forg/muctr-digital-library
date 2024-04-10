using MDLibrary.Areas.Admin.Models.ViewModels;
using MDLibrary.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MDLibrary.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class KeywordsController : Controller
	{
		private readonly MDLibraryBusinessDbContext _context;
		public KeywordsController(MDLibraryBusinessDbContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index(
			[FromQuery(Name = "f")] string? filter,
			[FromQuery(Name = "p")] int page = 1,
			[FromQuery(Name = "n")] int itemsPerPage = 20
			)
		{
			var keywords = _context.Keywords
				.OrderBy(k => k.KeywordId)
				.Select(k => k);

			if (!filter.IsNullOrEmpty())
			{
				ViewData["Filter"] = filter;
				int.TryParse(filter, out var id);
				keywords = from a in keywords
						   where
							(id != 0 && a.KeywordId == id) ||
							EF.Functions.Like(a.Value, $"%{filter}%")
						   select a;
			}

			var itemsCount = keywords.Count();
			var keywordsViewModels = await keywords
				.Skip((page - 1) * itemsPerPage)
				.Take(itemsPerPage)
				.Select(k => new KeywordsShortInfoViewModel
				{
					Id = k.KeywordId,
					Value = k.Value
				})
				.AsNoTracking()
				.ToListAsync();
			return View(new KeywordsIndexViewModel
			{
				Keywords = keywordsViewModels,
				PagingInfo = new MDLibrary.Models.PagingInfo
				{
					ItemsPerPage = itemsPerPage,
					TotalItems = itemsCount,
					CurrentPage = page
				}
			});
		}

		public IActionResult Delete(int? id, bool? saveChangesError = false)
		{
			if (id is null)
			{
				return new StatusCodeResult(StatusCodes.Status400BadRequest);
			}

			if (saveChangesError.GetValueOrDefault())
			{
				ViewBag.ErrorMessage = "Ошибка. Попробуйте еще раз или, в случае неудачи, свяжитесь с администратором";
			}

			var keyword = _context.Keywords.FirstOrDefault(a => a.KeywordId == id);
			if (keyword is null)
			{
				return NotFound();
			}

			return View(new KeywordsShortInfoViewModel
			{
				Id = keyword.KeywordId,
				Value = keyword.Value
			});
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{
			try
			{
				var keyword = _context.Keywords.FirstOrDefault(a => a.KeywordId == id);
				_context.Keywords.Remove(keyword!);
				_context.SaveChanges();
			}
			catch (DbUpdateException)
			{
				return RedirectToAction("Delete", new { id, saveChangesError = true });
			}
			return RedirectToAction("Index");
		}

		public IActionResult Edit(int? id, bool? saveChangesError = false)
		{
			if (id is null)
			{
				return new StatusCodeResult(StatusCodes.Status400BadRequest);
			}

			if (saveChangesError.GetValueOrDefault())
			{
				ViewBag.ErrorMessage = "Ошибка. Попробуйте еще раз или, в случае неудачи, свяжитесь с администратором";
			}

			var keyword = _context.Keywords.FirstOrDefault(a => a.KeywordId == id);
			if (keyword is null)
			{
				return NotFound();
			}
			return View(new KeywordsEditViewModel
			{
				Id = keyword.KeywordId,
				Value = keyword.Value
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(KeywordsEditViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var keywordToUpdate = await _context.Keywords
				.FirstOrDefaultAsync(a => a.KeywordId == model.Id);

			if (keywordToUpdate is null)
			{
				return RedirectToAction("Edit", new { model.Id, saveChangesError = true });
			}

			keywordToUpdate.Value = model.Value;
			_context.Keywords.Update(keywordToUpdate);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				return RedirectToAction("Edit", new { model.Id, saveChangesError = true });
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
