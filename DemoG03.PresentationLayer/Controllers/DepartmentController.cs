using DemoG03.BusinessLogic.Services;
using DemoG03.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoG03.PresentationLayer.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentServices _departmentServices;
        public DepartmentController(IDepartmentServices departmentServices)
        {
            _departmentServices = departmentServices;
        }
        public IActionResult Index()
        {
            var departments = _departmentServices.GetAllDepartments();
            return View(departments);
        }
    }
}
