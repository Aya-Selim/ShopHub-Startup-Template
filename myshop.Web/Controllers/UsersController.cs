using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.Business.Services.IServices;
using myshop.Web.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace myshop.Web.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            int pageSize = 5;
            if (page < 1) page = 1;

            var (users, totalCount) = await _userService.GetUsersPagedAsync(search, page, pageSize);

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            var vm = new UserIndexVM
            {
                Users = users,
                SearchTerm = search,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalCount,
                CurrentUserId = currentUserId
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var (success, message) = await _userService.ChangeRoleAsync(id, currentUserId);

            if (success)
            {
                TempData["Update"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLock(string id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var (success, message) = await _userService.ToggleLockoutAsync(id, currentUserId);

            if (success)
            {
                TempData["Update"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var (success, message) = await _userService.DeleteUserAsync(id, currentUserId);

            if (success)
            {
                TempData["Delete"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
