using MDLibrary.Areas.Identity.Models.ViewModels;
using MDLibrary.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MDLibrary.Areas.Identity.Controllers
{
	[Area("Identity")]
	[Authorize]
	public class AccountController : Controller
	{
		private readonly UserManager<LibraryUser> _userManager;

        public AccountController(UserManager<LibraryUser> userManager)
        {
            _userManager = userManager;
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

		public IActionResult Bookmarks()
		{
			return View();
		}
	}
}
