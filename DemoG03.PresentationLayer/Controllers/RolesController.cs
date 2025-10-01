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
    [Authorize]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateRoleViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            var exist = _roleManager.FindByNameAsync(viewModel.RoleName).Result;
            if (exist is null)
            {
                var result = _roleManager.CreateAsync(new IdentityRole
                {
                    Name = viewModel.RoleName,
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
                        (r.Id ?? "").Contains(UsersSearchName) ||
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
        public IActionResult Details(string? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));
            var Role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);
            if (Role == null) return RedirectToAction("Index");
            var VmRole = new RolesViewModel
            {
                Id = Role.Id,
                Name = Role.Name
            };
            return View(VmRole);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(string? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));
            var role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);
            if (role == null) return RedirectToAction(nameof(Index));

            var VmRole = new RolesViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(VmRole);
        }
        [HttpPost]
        public IActionResult Edit(RolesViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            var role = _roleManager.Roles.FirstOrDefault(x => x.Id == viewModel.Id);
            if (role == null) return View(viewModel);
            role.Name = viewModel.Name;
            var result = _roleManager.UpdateAsync(role).Result;
            if (result.Succeeded)
            {
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
            var role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);
            if (id is null || role is null) return RedirectToAction(nameof(Index));
            var vmRole = new RolesViewModel
            {
                Id = role.Id,
                Name = role.Name ?? "No Name"
            };
            return View(vmRole);
        }
        [HttpPost]
        public IActionResult Delete(RolesViewModel viewModel)
        {
            var role = _roleManager.Roles.FirstOrDefault(r => r.Id == viewModel.Id);
            if (viewModel.Id is null || role is null) return RedirectToAction(nameof(Index));

            var result = _roleManager.DeleteAsync(role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
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
