using DemoG03.BusinessLogic.DTOs.Employees;
using DemoG03.BusinessLogic.Services.Interfaces;
using DemoG03.DataAccess.Repositories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG03.BusinessLogic.Services.Classes
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<EmployeeDto> GetAllEmployees(bool withTracking = false)
        {
            var employees = _employeeRepository.GetAll(withTracking);
            var employeesToReturn = employees.Select(E => new EmployeeDto
            {
                Id = E.Id,
                Name = E.Name,
                Age = E.Age,
                Email = E.Email,
                EmployeeType = E.EmployeeType.ToString(),
                Gender = E.Gender.ToString(),
                IsActive = E.IsActive,
                Salary = E.Salary
            });
            return employeesToReturn;
        }

        public EmployeeDetailsDto? GetEmployeeById(int id)
        {
            var employee = _employeeRepository.GetById(id);
            return employee is null ? null: new EmployeeDetailsDto()
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Email = employee.Email,
                Salary = employee.Salary,
                IsActive = employee.IsActive,
                HiringDate = DateOnly.FromDateTime(employee.HiringDate),
                Gender = employee.Gender.ToString(),
                PhoneNumber = employee.PhoneNumber,
                CreatedBy = employee.CreatedBy,
                CreatedOn = employee.CreatedOn,
                EmployeeType = employee.EmployeeType.ToString(),
                LastModifiedBy = employee.LastModifiedBy,
                LastModifiedOn = employee.LastModifiedOn
            };
        }


        public int CreateEmployee(CreatedEmplyeeDto emplyeeDto)
        {
            throw new NotImplementedException();
        }

        public bool DeleteEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public int UpdateEmployee(UpdatedEmployeeDto employeeDto)
        {
            throw new NotImplementedException();
        }
    }
}
