using MDLibrary.Areas.Admin.Models.ViewModels;
using MDLibrary.Domain;
using MDLibrary.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace MDLibrary.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LiteratureController : Controller
    {
        private readonly MDLibraryBusinessDbContext _context;

        public LiteratureController(MDLibraryBusinessDbContext context)
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
                .OrderBy(x => x.LiteratureId)
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
                .Select(x => new LiteratureShortInfoViewModel
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

            var literatureEntity = _context.Literature.FirstOrDefault(x => x.LiteratureId == id);
            if (literatureEntity is null)
            {
                return NotFound();
            }

            return View(new LiteratureShortInfoViewModel
            {
                Id = literatureEntity.LiteratureId,
                Caption = literatureEntity.Caption
            });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var article = _context.Literature.FirstOrDefault(x => x.LiteratureId == id);
                _context.Literature.Remove(article!);
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

			var literatureEntity = _context.Literature
                .Include(x => x.Authors)
                .Include(x => x.Keywords)
                .FirstOrDefault(x => x.LiteratureId == id);
			if (literatureEntity is null)
			{
				return NotFound();
			}

            return View(new LiteratureEditViewModel
            {
                Id = literatureEntity.LiteratureId,
                PublishYear = literatureEntity.PublishYear,
                PageCount = literatureEntity.PageCount,
                Caption = literatureEntity.Caption,
                PublishLocation = literatureEntity.PublishLocation,
                Publisher = literatureEntity.Publisher,
                Isbn = literatureEntity.Isbn,
                Bbc = literatureEntity.Bbc,
                Udc = literatureEntity.Udc,
                Abstract = literatureEntity.Abstract,
                Keywords = literatureEntity.Keywords is null
                    ? null
                    : string.Join(", ", literatureEntity.Keywords),
				Authors = literatureEntity.Authors is null
					? null
					: string.Join(", ", literatureEntity.Authors)
			});
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LiteratureEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var literatureToUpdate = await _context.Literature
                .FirstOrDefaultAsync(x => x.LiteratureId == model.Id);

            if (literatureToUpdate == null)
            {
				return RedirectToAction("Edit", new { model.Id, saveChangesError = true });
			}

            literatureToUpdate.LiteratureId = model.Id;
            literatureToUpdate.PublishYear = model.PublishYear;
            literatureToUpdate.PageCount = model.PageCount;
            literatureToUpdate.Caption = model.Caption!;
            literatureToUpdate.PublishLocation = model.PublishLocation;
            literatureToUpdate.Publisher = model.Publisher;
            literatureToUpdate.Isbn = model.Isbn;
            literatureToUpdate.Bbc = model.Bbc;
            literatureToUpdate.Udc = model.Udc;
            literatureToUpdate.Abstract = model.Abstract;
            literatureToUpdate.Authors = [];
            literatureToUpdate.Keywords = [];

            var authors = model.Authors?.Split(", ") ?? [];
            var keywords = model.Keywords?.Split(", ") ?? [];

			foreach (var name in authors)
			{
				var authorFromDb = _context.Authors
					.Include(a => a.Literature)
					.FirstOrDefault(a => a.Name == name);

				if (authorFromDb is null)
				{
					authorFromDb = new Author { Name = name };
					_context.Authors.Add(authorFromDb);
				}

				authorFromDb.Literature.Add(literatureToUpdate);
				literatureToUpdate.Authors.Add(authorFromDb);
			}

			foreach (var keyword in keywords)
			{
				var keywordFromDb = _context.Keywords
					.Include(k => k.Literature)
					.FirstOrDefault(k => k.Value == keyword);

				if (keywordFromDb is null)
				{
					keywordFromDb = new Keyword { Value = keyword };
					_context.Keywords.Add(keywordFromDb);
				}

				keywordFromDb.Literature.Add(literatureToUpdate);
				literatureToUpdate.Keywords.Add(keywordFromDb);
			}

            _context.Literature.Update(literatureToUpdate);
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

        public IActionResult Details(int? id)
        {
			if (id is null)
			{
				return new StatusCodeResult(StatusCodes.Status400BadRequest);
			}

            var literatureEntity = _context.Literature
                .Include(x => x.Authors)
                .Include(x => x.Keywords)
                .AsNoTracking()
                .FirstOrDefault(x => x.LiteratureId == id);

            if (literatureEntity is null)
            {
                return NotFound();
            }

			return View(new LiteratureDetailsViewModel
			{
				Id = literatureEntity.LiteratureId,
				PublishYear = literatureEntity.PublishYear,
				PageCount = literatureEntity.PageCount,
				Caption = literatureEntity.Caption,
				PublishLocation = literatureEntity.PublishLocation,
				Publisher = literatureEntity.Publisher,
				Isbn = literatureEntity.Isbn,
				Bbc = literatureEntity.Bbc,
				Udc = literatureEntity.Udc,
				Abstract = literatureEntity.Abstract,
				Keywords = literatureEntity.Keywords is null
					? null
					: string.Join(", ", literatureEntity.Keywords),
				Authors = literatureEntity.Authors is null
					? null
					: string.Join(", ", literatureEntity.Authors)
			});
		}

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(LiteratureAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

			var literatureToCreate = new Literature
			{
				PublishYear = model.PublishYear,
				PageCount = model.PageCount,
				Caption = model.Caption!,
				PublishLocation = model.PublishLocation,
				Publisher = model.Publisher,
				Isbn = model.Isbn,
				Bbc = model.Bbc,
				Udc = model.Udc,
				Abstract = model.Abstract,
				Authors = [],
				Keywords = []
			};

			var authors = model.Authors?.Split(", ") ?? [];
			var keywords = model.Keywords?.Split(", ") ?? [];

			foreach (var name in authors)
			{
				var authorFromDb = _context.Authors
					.Include(a => a.Literature)
					.FirstOrDefault(a => a.Name == name);

				if (authorFromDb is null)
				{
					authorFromDb = new Author { Name = name };
					_context.Authors.Add(authorFromDb);
				}

				authorFromDb.Literature.Add(literatureToCreate);
				literatureToCreate.Authors.Add(authorFromDb);
			}

			foreach (var keyword in keywords)
			{
				var keywordFromDb = _context.Keywords
					.Include(k => k.Literature)
					.FirstOrDefault(k => k.Value == keyword);

				if (keywordFromDb is null)
				{
					keywordFromDb = new Keyword { Value = keyword };
					_context.Keywords.Add(keywordFromDb);
				}

				keywordFromDb.Literature.Add(literatureToCreate);
				literatureToCreate.Keywords.Add(keywordFromDb);
			}

            if (model.LiteratureFile is not null)
            {
                try
                {
                    var filePath = Path.Combine(LiteratureFile.RootPath, model.LiteratureFile.FileName);
				    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        await model.LiteratureFile.CopyToAsync(fs);
                    }
                    var literatureFileToCreate = new LiteratureFile
                    {
                        Literature = literatureToCreate,
                        Extension = "",
                        Filename = model.LiteratureFile.FileName
                    };
                    _context.LiteratureFiles.Add(literatureFileToCreate);
                    _ExtractTextFromPdfToDb(literatureToCreate, model.LiteratureFile.FileName);
                }
                catch (SystemException)
                {
					ModelState.AddModelError("", "Возникла ошибка при сохранении данных. Попробуйте еще раз.");
					return View();
				}
			}

			_context.Literature.Add(literatureToCreate);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
                ModelState.AddModelError("", "Возникла ошибка при сохранении данных. Попробуйте еще раз.");
				return View();
			}
			return RedirectToAction(nameof(Details), new { id = literatureToCreate.LiteratureId });
		}

		private void _ExtractTextFromPdfToDb(Literature literature, string fileName)
		{
			var path = Path.Combine(LiteratureFile.RootPath, fileName);
			var pdfReader = new PdfReader(path);
			var pdfDocument = new PdfDocument(pdfReader);

			for (short page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
			{
				ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

                _context.LiteraturePages.Add(new LiteraturePage
                {
                    Literature = literature,
                    PageNumber = page,
                    Text = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page), strategy)
                });
			}
		}
	}
}
