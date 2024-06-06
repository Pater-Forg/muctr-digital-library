using MDLibrary.Domain;
using MDLibrary.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;

namespace MDLibrary.Controllers
{
	public class ReaderController : Controller
	{
		private readonly MDLibraryBusinessDbContext _context;

        public ReaderController(MDLibraryBusinessDbContext context)
        {
			_context = context;
        }

		[ActionName("View")]
        public IActionResult Display(int? id, short page = 1)
		{
			if (id is null)
			{
				return new StatusCodeResult(StatusCodes.Status400BadRequest);
			}
			var file = _context.LiteratureFiles
							   .Include(f => f.Literature)
							   .FirstOrDefault(f => f.Literature.LiteratureId == id);
			if (file is null)
			{
				return NotFound();
			}
			ViewBag.FileId = file.LiteratureFileId;
			ViewBag.LiteratureId = id;
			ViewBag.InitPage = page;
							
			return View();
		}

		public IActionResult GetFile(int id)
		{
			var file = _context.LiteratureFiles
							   .FirstOrDefault(f => f.LiteratureFileId == id);
			if (file is null)
			{
				return NotFound();
			}
			var path = Path.Join(LiteratureFile.RootPath, file.Filename);
			FileStream fileStream = System.IO.File.OpenRead(path);
			return File(fileStream, "application/octet-stream");
		}
	}
}
