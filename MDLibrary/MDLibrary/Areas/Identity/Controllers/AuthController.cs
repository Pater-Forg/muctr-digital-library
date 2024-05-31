using MDLibrary.Areas.Identity.Models.ViewModels;
using MDLibrary.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;

namespace MDLibrary.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AuthController : Controller
    {
        private readonly SignInManager<LibraryUser>     _signInManager;
        private readonly UserManager<LibraryUser>       _userManager;
        private readonly IUserStore<LibraryUser>        _userStore;
        private readonly IUserEmailStore<LibraryUser>   _emailStore;
        private readonly IEmailSender<LibraryUser>      _emailSender;

        public AuthController(
            SignInManager<LibraryUser> signInManager,
            UserManager<LibraryUser> userManager,
            IUserStore<LibraryUser> userStore,
            IEmailSender<LibraryUser> emailSender
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailSender = emailSender;
            _emailStore = (IUserEmailStore<LibraryUser>)_userStore;
		}

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _signInManager
                .PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                ModelState.AddModelError("", "Не удалось произвести вход");
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = Activator.CreateInstance<LibraryUser>();
			await _userStore.SetUserNameAsync(user, userModel.UserName, CancellationToken.None);
			await _emailStore.SetEmailAsync(user, userModel.Email, CancellationToken.None);
			var result = await _userManager.CreateAsync(user, userModel.Password);

			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.TryAddModelError(error.Code, error.Description);
				}
				return View();
			}

			var userId = await _userManager.GetUserIdAsync(user);
			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action("ConfirmEmail", "Auth", new
            {
                area = "Identity",
                userId = userId,
                code = code
            }, protocol: Request.Scheme);

            //await _emailSender.SendConfirmationLinkAsync(user, userModel.Email, callbackUrl!);

            //return RedirectToAction("RegisterConfirmation", new { email = userModel.Email });

            // Email confirmation is under development. Code below is work around for the current moment
            return RedirectToAction("ConfirmEmail", new {
				userId = userId,
				code = code
			});
		}

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
			if (userId == null || code == null)
			{
				return RedirectToAction("Index", "Home", new { area = "" });
			}

			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return NotFound();
			}

			code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
			var result = await _userManager.ConfirmEmailAsync(user, code);
            ViewData["Status"] = result.Succeeded;
			return View();
		}
    }
}
