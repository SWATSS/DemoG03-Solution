using Microsoft.AspNetCore.Mvc;

namespace DemoG03.PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        // Register
        public IActionResult Register()
        {
            return View();
        }
        // Login
        public IActionResult LogIn()
        {
            return View();
        }
    }
}
