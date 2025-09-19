using DemoG03.BusinessLogic.DTOs.Employees;
using DemoG03.BusinessLogic.Services.Interfaces;
using DemoG03.DataAccess.Models.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace DemoG03.PresentationLayer.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public EmployeesController(IEmployeeService employeeService,
                                    ILogger<IEmployeeService> logger,
                                    IWebHostEnvironment env)
        {
            _employeeService = employeeService;
            _logger = logger;
            _env = env;
        }
        public IActionResult Index()
        {
            var employees = _employeeService.GetAllEmployees();
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreatedEmployeeDto employeeDto)
        {
            //Server-Side Validation
            if (ModelState.IsValid)
            {
                try
                {
                    var Result = _employeeService.CreateEmployee(employeeDto);
                    if (Result > 0) return RedirectToAction(nameof(Index));
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Cant't Create Employee");
                    }
                }
                catch (Exception ex)
                {
                    if (_env.IsDevelopment())
                        ModelState.AddModelError(string.Empty, "Cant't Create Employee");
                    else
                        _logger.LogError(ex.Message);
                }
            }
            return View(employeeDto);
        }
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null) return BadRequest();// 400 Client-Side Error
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();// 404 Client-Side Error
            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest();// 400
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();// 404

            return View(new UpdatedEmployeeDto()
            {
                Id = employee.Id,
                Name = employee.Name,
                Address = employee.Address,
                Age = employee.Age,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                Salary = employee.Salary,
                IsActive = employee.IsActive,
                HiringDate = employee.HiringDate,
                Gender = Enum.Parse<Gender>(employee.Gender),
                EmployeeType = Enum.Parse<EmployeeType>(employee.EmployeeType)
            });
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int? id, UpdatedEmployeeDto employeeDto)
        {
            if (id is null || id != employeeDto.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(employeeDto);
            }
            try
            {
                var Result = _employeeService.UpdateEmployee(employeeDto);
                if (Result > 0) return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, "Employee Is Not Updated");
                }
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment()) ModelState.AddModelError(string.Empty, ex.Message);
                else
                    _logger.LogError(ex.Message);
            }
            return View(employeeDto);

        }

        [HttpPost]
        //[ValidateAntiForgeryToken] // ActionFilter
        public IActionResult Delete([FromRoute] int? id)
        {
            if (id is null) return BadRequest();//400
            try
            {
                var result = _employeeService.DeleteEmployee(id.Value);
                if (result)
                    return RedirectToAction(nameof(Index));
                else
                    _logger.LogError("Employee Can't Be Deleted");
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment()) ModelState.AddModelError(string.Empty, ex.Message);
                else
                    _logger.LogError(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
