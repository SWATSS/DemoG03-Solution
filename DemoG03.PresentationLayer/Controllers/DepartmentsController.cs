using DemoG03.BusinessLogic.DataTransferObjects;
using DemoG03.BusinessLogic.Services;
using DemoG03.DataAccess.Models;
using DemoG03.PresentationLayer.Models.Departments;
using Microsoft.AspNetCore.Mvc;

namespace DemoG03.PresentationLayer.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentServices _departmentServices;
        private readonly ILogger<DepartmentsController> _logger;
        private readonly IWebHostEnvironment _env;
        public DepartmentsController(IDepartmentServices departmentServices, ILogger<DepartmentsController> logger, IWebHostEnvironment env)
        {
            _departmentServices = departmentServices;
            _logger = logger;
            _env = env;
        }
        public IActionResult Index()
        {
            var departments = _departmentServices.GetAllDepartments();
            return View(departments);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreatedDepartmentDto departmentDto)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentDto);
            }
            var message = string.Empty;
            try
            {
                var result = _departmentServices.AddDepartment(departmentDto);
                if (result > 0)
                {
                    return View("Index");
                }
                else
                {
                    message = "Department Cannot be Created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(departmentDto);
                }
            }
            catch (Exception ex)
            {
                // Log Exception
                _logger.LogError(ex, ex.Message);
                if (_env.IsDevelopment())

                {
                    message = ex.Message;
                    return View(departmentDto);
                }
                else

                {
                    message = "Deaprtment cannot be created";
                    return View("Error", message);
                }
            }
        }

        [HttpGet]
        // baseUrl/Department/Details/{Id}
        public IActionResult Details(int? id)
        {

            if (id is null)
            {
                return BadRequest(); // 400
            }

            var department = _departmentServices.GetDepartmentById(id.Value);
            if (department is null)
            {
                return NotFound(); // 404
            }

            return View(department);
        }

        // Get: baseUrl/Departments/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest(); // 400
            }
            var department = _departmentServices.GetDepartmentById(id.Value);
            if (department is null)
            {
                return NotFound(); // 404
            }
            return View(new DepartmentViewModel()
            {
                Code = department.Code,
                Name = department.Name,
                Description = department.Description,
                DateofCreation = (DateOnly)department.DateOfCreation
            });
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            var message = string.Empty;
            try
            {
                var result = _departmentServices.UpdateDepartment(new UpdatedDepartmentDto()
                {
                    Id = id,
                    Code = departmentVM.Code,
                    Name = departmentVM.Name,
                    Description = departmentVM.Description,
                    DateOfCreation = departmentVM.DateofCreation
                });
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Department Cant Be Updated";
                    return View(departmentVM);
                }
            }
            catch (Exception ex)
            {
                message = _env.IsDevelopment() ? ex.Message : "Department Cant Be Updated";
            }
            return View(departmentVM);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null)
                return NotFound(); // 404
            var department = _departmentServices.GetDepartmentById(id.Value);
            if (department is null)
                return BadRequest(); // 400

            return View(department);
        }
    }
}


