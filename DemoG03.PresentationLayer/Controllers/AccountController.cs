using DemoG03.DataAccess.Models.IdentityModels;
using DemoG03.PresentationLayer.Utilities;
using DemoG03.PresentationLayer.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Common;

namespace DemoG03.PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region Register
        // Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByNameAsync(viewModel.UserName).Result;
                if (user is null)
                {
                    // Create Account
                    user = new ApplicationUser()
                    {
                        UserName = viewModel.UserName,
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        Email = viewModel.Email
                    };

                    var result = _userManager.CreateAsync(user, viewModel.Password).Result;
                    if (result.Succeeded)
                    {
                        RedirectToAction(nameof(LogIn));
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
                    ModelState.AddModelError(string.Empty, "UserName Is Used");
                }
            }
            return View(viewModel);
        }
        #endregion

        #region LogIn
        // Login
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LogIn(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByEmailAsync(viewModel.Email).Result;
                if (user is not null)
                {
                    var flag = _userManager.CheckPasswordAsync(user, viewModel.Password).Result;
                    if (flag)
                    {
                        var result = _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, false).Result;
                        if (result.IsNotAllowed)
                        {
                            ModelState.AddModelError(string.Empty, "Your Account is not Allowed");
                        }
                        else if (result.IsLockedOut)
                        {
                            ModelState.AddModelError(string.Empty, "Your Account is Locked out");
                        }
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Email or Password is Invalid");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email or Password is Invalid");
                }
            }
            return View(viewModel);
        }
        #endregion

        #region LogOut
        [Authorize]
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();// handel Async
            return RedirectToAction(nameof(LogIn));
        }
        #endregion

        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendResetPasswordLink(ForgetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByEmailAsync(viewModel.Email).Result;
                if (user is not null)
                {
                    var Token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                    var resetPasswordLink = Url.Action(nameof(ResetPassword), "Account", new { email = viewModel.Email, Token }, Request.Scheme); // Token Needs Service

                    // Create Email Messege
                    var email = new Email()
                    {
                        To = viewModel.Email,
                        Subject = "Reset Password",
                        Body = resetPasswordLink
                    };
                    // Send Email
                    EmailSettings.SendEmail(email);
                    TempData["Email"] = viewModel.Email;
                    return RedirectToAction(nameof(CheckYourInbox));
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Operation");
            return View(nameof(ForgetPassword), viewModel);
        }
        public IActionResult CheckYourInbox()
        {
            var model = new ForgetPasswordViewModel
            {
                Email = TempData["Email"]?.ToString()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string Token)
        {
            TempData["email"] = email;
            TempData["Token"] = Token;
            if (email is null || Token is null)
                return RedirectToAction(nameof(ForgetPassword));

            var user = _userManager.FindByEmailAsync(email).Result;
            if (user == null)
                return RedirectToAction(nameof(ForgetPassword));
            var isValid = _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", Token).Result;

            if (!isValid)
            {
                TempData["ErrorMessage"] = "Token Expierd";
                return RedirectToAction(nameof(ForgetPassword));
            }

            return View();
        }
        // Password Pattren That's Follow Password Needs:
        // Pa$$w0rd
        // P@ssw0rd
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            string email = TempData["email"] as string ?? string.Empty;
            string Token = TempData["Token"] as string ?? string.Empty;

            var user = _userManager.FindByEmailAsync(email).Result;
            if (user is not null)
            {
                var Result = _userManager.ResetPasswordAsync(user, Token, viewModel.Password).Result;
                if (Result.Succeeded) return RedirectToAction(nameof(LogIn));
                else
                {
                    foreach (var error in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(viewModel);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

