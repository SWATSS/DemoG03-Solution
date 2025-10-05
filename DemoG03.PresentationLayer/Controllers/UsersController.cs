using DemoG03.DataAccess.Models.IdentityModels;
using DemoG03.PresentationLayer.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemoG03.PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #region GetAll
        public IActionResult Index(string? UsersSearchName)
        {
            var query = _userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(UsersSearchName))
            {
                query = query.Where(u =>
                        (u.FirstName ?? "").Contains(UsersSearchName) ||
                        (u.LastName ?? "").Contains(UsersSearchName) ||
                        (u.Email ?? "").Contains(UsersSearchName) ||
                        (u.PhoneNumber ?? "").Contains(UsersSearchName)
                        );
            }
            var users = query.Select(u => new UsersViewModel
            {
                id = u.Id,
                Email = u.Email,
                FName = u.FirstName,
                LName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                Roles = _userManager.GetRolesAsync(u).Result
            }).ToList();

            return View(users);
        }
        #endregion

        #region Details
        public IActionResult Details(string? id, string viewName = "Details")
        {
            if (id == null) return NotFound();
            var User = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (User.Id != id) return BadRequest();
            var allRoles = _roleManager.Roles.ToList();
            var viewModel = new UsersViewModel
            {
                id = User.Id,
                FName = User.FirstName,
                LName = User.LastName,
                Email = User.Email,
                PhoneNumber = User.PhoneNumber,
                Roles = _userManager.GetRolesAsync(User).Result,
            };
            ViewBag.AvailableRoles = allRoles;
            return View(viewModel);
        }
        #endregion

        #region Edit

        [HttpGet]
        public IActionResult Edit(string id)
        {
            ///var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            ///if (user is null)
            ///{
            ///    return RedirectToAction(nameof(Index));
            ///}
            ///var viewModel = new UserEditViewModel
            ///{
            ///    FName = user.FirstName,
            ///    LName = user.LastName ?? "Last Name",
            ///    PhoneNumber = user.PhoneNumber ?? "No Number",
            ///    Roles = _userManager.GetRolesAsync(user).Result
            ///};
            ///TempData["UserId"] = user.Id;
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] string? id, UsersViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            //var UserId = TempData["UserId"].ToString();
            var user = _userManager.FindByIdAsync(id).Result;
            if (user is null) return RedirectToAction(nameof(Index));

            user.FirstName = viewModel.FName;
            user.LastName = viewModel.LName;
            user.PhoneNumber = viewModel.PhoneNumber;

            var result = _userManager.UpdateAsync(user).Result;
            if (result.Succeeded)
            {
                TempData["Message"] = "Edited Succissfully";
                return RedirectToAction(nameof(Details), routeValues: id);
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(viewModel);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete([FromRoute] string? id)
        {
            ///if (string.IsNullOrEmpty(id)) return RedirectToAction(nameof(Index));
            ///var user = _userManager?.FindByIdAsync(id).Result;
            ///if (user is null) return RedirectToAction(nameof(Index));
            ///var vm = new UsersViewModel
            ///{
            ///    id = user.Id,
            ///    FName = user.FirstName,
            ///    LName = user.LastName,
            ///    Email = user.Email,
            ///    PhoneNumber = user.PhoneNumber,
            ///    Roles = _userManager.GetRolesAsync(user).Result
            ///};
            return Details(id, "Delete");
        }

        //[HttpPost, ActionName("Delete")] // For Keep it With the Same Name .
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] string? id, UsersViewModel viewModel)
        {
            if (id is null || id != viewModel.id) return BadRequest();
            var user = _userManager?.FindByIdAsync(id).Result;

            if (user is null) return NotFound();

            var result = _userManager.DeleteAsync(user).Result;
            if (result.Succeeded)
            {
                TempData["Message"] = "Deleted Succissfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }
        #endregion
    }
}
