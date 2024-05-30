using MDLibrary.Areas.Admin.Models.ViewModels;
using MDLibrary.Domain.Entities;
using MDLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAS.Demo.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admins")]
	public class UsersController : Controller
	{
		private readonly UserManager<LibraryUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public int ItemsPerPage = 20;

		public UsersController(UserManager<LibraryUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<IActionResult> Index([FromQuery(Name = "f")] string? filter, int page = 1)
		{
			var users = _userManager.Users
				.OrderBy(u => u.Id)
				.Select(u => u);

			if (!filter.IsNullOrEmpty())
			{
				ViewData["Filter"] = filter;
				var isId = Guid.TryParse(filter, out var guid);
				users = from u in users
						where
							(isId && u.Id == filter) ||
							EF.Functions.ILike(u.UserName, $"%{filter}%") ||
							EF.Functions.ILike(u.Email, $"%{filter}%")
						select u;
			}
			var itemsCount = users.Count();
			var usersViewModels = await users
				.Skip((page - 1) * ItemsPerPage)
				.Take(ItemsPerPage)
				.Select(u => new UserViewModel
				{
					Id = u.Id,
					UserName = u.UserName,
					Email = u.Email,
				})
				.AsNoTracking()
				.ToListAsync();
			return View(new UsersIndexViewModel
			{
				Users = usersViewModels,
				PagingInfo = new PagingInfo
				{
					ItemsPerPage = ItemsPerPage,
					TotalItems = itemsCount,
					CurrentPage = page
				}
			});
		}

		public async Task<IActionResult> Details(string? id)
		{
			if (id is null)
			{
				return new StatusCodeResult(StatusCodes.Status400BadRequest);
			}

			var user = await _userManager.FindByIdAsync(id);
			if (user is null)
			{
				return NotFound();
			}

			var roles = await _userManager.GetRolesAsync(user);

			return View(new UserDetailsViewModel
			{
				Id = user.Id,
				Name = user.UserName,
				Email = user.Email,
				EmailConfirmed = user.EmailConfirmed,
				PhoneNumber = user.PhoneNumber,
				PhoneNumberConfirmed = user.PhoneNumberConfirmed,
				RoleCheckboxes = _roleManager.Roles.Select(r => new RoleCheckboxesViewModel
				{
					RoleId = r.Id,
					RoleName = r.Name,
					IsChecked = roles.Contains(r.Name)
				}).OrderBy(r => r.RoleName).ToList()
			});
		}

		[HttpPost]
		public async Task<IActionResult> Details(UserDetailsViewModel userViewModel)
		{
			var user = await _userManager.FindByIdAsync(userViewModel.Id);
			foreach (var roleViewModel in userViewModel.RoleCheckboxes)
			{
				var isInRole = await _userManager.IsInRoleAsync(user, roleViewModel.RoleName);
				// if role is checked and user is not in role then add user to role
				if (roleViewModel.IsChecked && !isInRole)
				{
					await _userManager.AddToRoleAsync(user, roleViewModel.RoleName);
				}
				// if role is not checked and user is in role then remove from role
				if (!roleViewModel.IsChecked && isInRole)
				{
					await _userManager.RemoveFromRoleAsync(user, roleViewModel.RoleName);
				}
			}
			return RedirectToAction(actionName: "Details", new { id = userViewModel.Id });
		}

		public async Task<IActionResult> Delete(string? id, bool? saveChangesError = false)
		{
			if (id is null)
			{
				return new StatusCodeResult(StatusCodes.Status400BadRequest);
			}

			if (saveChangesError.GetValueOrDefault())
			{
				ViewBag.ErrorMessage = "Ошибка. Попробуйте еще раз или, в случае неудачи, свяжитесь с администратором";
			}

			var user = await _userManager.FindByIdAsync(id);
			if (user is null)
			{
				return NotFound();
			}

			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			var result = await _userManager.DeleteAsync(user);

			if (!result.Succeeded)
			{
				return RedirectToAction("Delete", new { id, saveChangesError = true });
			}
			return RedirectToAction("Index");
		}
	}
}
