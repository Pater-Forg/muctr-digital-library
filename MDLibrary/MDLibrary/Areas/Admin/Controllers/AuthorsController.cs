using MDLibrary.Areas.Admin.Models.ViewModels;
using MDLibrary.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MDLibrary.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admins")]
	public class AuthorsController : Controller
	{
		private readonly MDLibraryBusinessDbContext _context;
		public AuthorsController(MDLibraryBusinessDbContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index(
			[FromQuery(Name = "f")] string? filter,
			[FromQuery(Name = "p")] int page = 1,
			[FromQuery(Name = "n")] int itemsPerPage = 20
			)
		{
			var authors = _context.Authors
				.OrderBy(a => a.AuthorId)
				.Select(a => a);

			if (!filter.IsNullOrEmpty())
			{
				ViewData["Filter"] = filter;
				int.TryParse(filter, out var id);
				authors = from a in authors
						  where
							(id != 0 && a.AuthorId == id) ||
							EF.Functions.Like(a.Name, $"%{filter}%")
						  select a;
			}

			var itemsCount = authors.Count();
			var authorsViewModels = await authors
				.Skip((page - 1) * itemsPerPage)
				.Take(itemsPerPage)
				.Select(a => new AuthorsShortInfoViewModel
				{
					Id = a.AuthorId,
					Name = a.Name
				})
				.AsNoTracking()
				.ToListAsync();
			return View(new AuthorsIndexViewModel
			{
				Authors = authorsViewModels,
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

			var author = _context.Authors.FirstOrDefault(a => a.AuthorId == id);
			if (author is null)
			{
				return NotFound();
			}

			return View(new AuthorsShortInfoViewModel
			{
				Id = author.AuthorId,
				Name = author.Name
			});
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{
			try
			{
				var author = _context.Authors.FirstOrDefault(a => a.AuthorId == id);
				_context.Authors.Remove(author!);
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

			var author = _context.Authors.FirstOrDefault(a => a.AuthorId == id);
			if (author is null)
			{
				return NotFound();
			}
			return View(new AuthorsEditViewModel
			{
				Id = author.AuthorId,
				Name = author.Name
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(AuthorsEditViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var authorToUpdate = await _context.Authors
				.FirstOrDefaultAsync(a => a.AuthorId == model.Id);

			if (authorToUpdate is null)
			{
				return RedirectToAction("Edit", new { model.Id, saveChangesError = true });
			}

			authorToUpdate.Name = model.Name;
			_context.Authors.Update(authorToUpdate);
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

		public IActionResult Details(int? id) {
			if (id is null)
			{
				return new StatusCodeResult(StatusCodes.Status400BadRequest);
			}

			var author = _context.Authors
				.Include(a => a.Literature)
				.AsNoTracking()
				.FirstOrDefault(a => a.AuthorId == id);

			if (author is null)
			{
				return NotFound();
			}

			return View(new AuthorsDetailsViewModel
			{
				Id = author.AuthorId,
				Name = author.Name,
				Literature = author.Literature.Select(
					x => new KeyValuePair<int, string>(x.LiteratureId, x.ToString()))
					.ToDictionary()
			});
		}
	}
}
