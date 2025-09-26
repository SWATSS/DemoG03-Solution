using DemoG03.BusinessLogic.DTOs.Employees;
using DemoG03.BusinessLogic.Services.Interfaces;
using DemoG03.DataAccess.Models.Departments;
using DemoG03.DataAccess.Models.Employees;
using DemoG03.PresentationLayer.ViewModels.Employees;
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
        public IActionResult Index(string? EmployeeSearchName)
        {
            var employees = _employeeService.GetAllEmployees(EmployeeSearchName);
            return View(employees);
        }
        [HttpGet]
        //public IActionResult Create([FromServices]IDepartmentServices departmentServices)//Depandancy Injection
        public IActionResult Create()
        {
            //ViewData["Departments"] = departmentServices.GetAllDepartments();
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            //Server-Side Validation
            if (ModelState.IsValid)
            {
                try
                {
                    var employeeDto = new CreatedEmployeeDto()
                    {
                        Name = employeeVM.Name,
                        Address = employeeVM.Address,
                        Age = employeeVM.Age,
                        Email = employeeVM.Email,
                        EmployeeType = employeeVM.EmployeeType,
                        Gender = employeeVM.Gender,
                        HiringDate = employeeVM.HiringDate,
                        IsActive = employeeVM.IsActive,
                        PhoneNumber = employeeVM.PhoneNumber,
                        Salary = employeeVM.Salary,
                        Image = employeeVM.Image
                    };
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
            return View(employeeVM);
        }
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null) return BadRequest();// 400 Client-Side Error
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();// 404 Client-Side Error
            return View(employee);
        }

        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest();// 400
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();// 404

            return View(new EmployeeViewModel()
            {
                Name = employee.Name,
                Address = employee.Address,
                Age = employee.Age,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                Salary = employee.Salary,
                IsActive = employee.IsActive,
                HiringDate = employee.HiringDate,
                Gender = Enum.Parse<Gender>(employee.Gender),
                EmployeeType = Enum.Parse<EmployeeType>(employee.EmployeeType),
                DepartmentId = employee.DepartmentId,
                Image = employee.Image
            });
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int? id, EmployeeViewModel employeeVM)
        {
            if (id is null) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var employeeDto = new UpdatedEmployeeDto()
                    {
                        Id = id.Value,
                        Name = employeeVM.Name,
                        Address = employeeVM.Address,
                        Salary = employeeVM.Salary,
                        IsActive = employeeVM.IsActive,
                        PhoneNumber = employeeVM.PhoneNumber,
                        Email = employeeVM.Email,
                        HiringDate = employeeVM.HiringDate,
                        Age = employeeVM.Age,
                        EmployeeType = employeeVM.EmployeeType,
                        Gender = employeeVM.Gender,
                        DepartmentId = employeeVM.DepartmentId,
                        Image = employeeVM.Image
                    };
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
            }
            return View(employeeVM);

        }
        #endregion

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
