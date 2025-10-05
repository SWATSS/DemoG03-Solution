using DemoG03.DataAccess.Models.IdentityModels;
using DemoG03.PresentationLayer.ViewModels.Roles;
using DemoG03.PresentationLayer.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Data;

namespace DemoG03.PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager,
                                UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        #region Create
        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult Create()
        {
            return View();
        }
        ///public IActionResult Create(string RoleName)
        ///{
        ///    if (!ModelState.IsValid) return View(RoleName);
        ///    var exist = _roleManager.FindByNameAsync(RoleName).Result;
        ///    if (exist is null)
        ///    {
        ///        var result = _roleManager.CreateAsync(new IdentityRole
        ///        {
        ///            Name = RoleName
        ///        }).Result;
        ///        if (result.Succeeded)
        ///        {
        ///            TempData["Message"] = "Created Successfully";
        ///            return RedirectToAction(nameof(Index));
        ///        }
        ///        else
        ///        {
        ///            foreach (var error in result.Errors)
        ///            {
        ///                ModelState.AddModelError(string.Empty, error.Description);
        ///            }
        ///        }
        ///    }
        ///    else
        ///    {
        ///        ModelState.AddModelError(string.Empty, "Name Is Used");
        ///    }
        ///    return View(RoleName);
        ///}
        [HttpPost]
        public IActionResult Create(RolesViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            var exist = _roleManager.FindByNameAsync(viewModel.Name).Result;
            if (exist is null)
            {
                var result = _roleManager.CreateAsync(new IdentityRole
                {
                    Name = viewModel.Name,
                }).Result;
                if (result.Succeeded)
                {
                    TempData["Message"] = "Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Name Is Used");
            }
            return View(viewModel);
        }
        #endregion

        #region GetAll
        public IActionResult Index(string? UsersSearchName)
        {
            var query = _roleManager.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(UsersSearchName))
            {
                query = query.Where(r =>
                        (r.Name ?? "").Contains(UsersSearchName)
                        );
            }
            var roles = query.Select(r => new RolesViewModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();


            return View(roles);
        }
        #endregion

        #region Details
        public IActionResult Details(string? id, string viewName = "Details")
        {
            if (id == null) return BadRequest();
            //var Role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);
            var Role = _roleManager.FindByIdAsync(id).Result;
            if (Role == null) return NotFound();
            var VmRole = new RolesViewModel
            {
                Id = Role.Id,
                Name = Role.Name
            };
            return View(viewName, VmRole);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(string? id)
        {
            ///if (id == null) return RedirectToAction(nameof(Index));
            ///var role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);
            ///if (role == null) return RedirectToAction(nameof(Index));

            ///var VmRole = new RolesViewModel
            ///{
            ///    Id = role.Id,
            ///    Name = role.Name
            ///};
            ///return View(VmRole);

            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] string? id, RolesViewModel viewModel)
        {
            if (id is null || id != viewModel.Id) return BadRequest();

            if (!ModelState.IsValid) return View(viewModel);
            var role = _roleManager.FindByIdAsync(id).Result;
            if (role is null) return NotFound();

            role.Name = viewModel.Name;

            var result = _roleManager.UpdateAsync(role).Result;
            if (result.Succeeded)
            {
                TempData["Message"] = "Edited Succissfully";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(viewModel);
        }
        #endregion

        #region Delete
        public IActionResult Delete(string? id)
        {
            ///var role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);
            ///if (id is null || role is null) return RedirectToAction(nameof(Index));
            ///var vmRole = new RolesViewModel
            ///{
            ///    Id = role.Id,
            ///    Name = role.Name ?? "No Name"
            ///};
            return View(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] string? id, RolesViewModel viewModel)
        {
            if (id is null || id != viewModel.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var role = _roleManager.FindByIdAsync(id).Result;
                if (role is null) return NotFound();

                if (viewModel.Id is null || role is null) return RedirectToAction(nameof(Index));

                var result = _roleManager.DeleteAsync(role).Result;
                if (result.Succeeded)
                {
                    TempData["Message"] = "Deleted Succissfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View();
        }
        #endregion

        public IActionResult AddOrRemoveUsers(string? roleId)
        {
            if (roleId is null) return BadRequest();
            var role = _roleManager.FindByIdAsync(roleId).Result;
            if (role is null) return NotFound();

            ViewData["RoleId"] = roleId;
            var usersInRole = new List<UserInRoleViewModel>();
            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                var userInRole = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (_userManager.IsInRoleAsync(user, role.Name).Result)
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }
                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
        }

        [HttpPost]
        public IActionResult AddOrRemoveUsers(string? roleId, List<UserInRoleViewModel> users)
        {
            if (roleId is null) return BadRequest();

            var role = _roleManager.FindByIdAsync(roleId).Result;
            if (role is null) return NotFound();

            if (ModelState.IsValid)
            {
                bool flag = true;
                IdentityResult result = null;
                foreach (var user in users)
                {
                    var appUser = _userManager.FindByIdAsync(user.UserId).Result;
                    if (appUser is not null)
                    {
                        if (user.IsSelected && !_userManager.IsInRoleAsync(appUser, role.Name).Result)
                        {
                            result = _userManager.AddToRoleAsync(appUser, role.Name).Result;
                        }
                        else if (!user.IsSelected && _userManager.IsInRoleAsync(appUser, role.Name).Result)
                        {
                            result = _userManager.RemoveFromRoleAsync(appUser, role.Name).Result;
                        }
                        if (result is not null && !result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            flag = false;
                            result = null;
                        }
                    }
                }
                if (flag) return RedirectToAction(nameof(Edit), new { id = roleId });
            }
            return View(users);
        }
    }
}
