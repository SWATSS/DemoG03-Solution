using AutoMapper;
using DemoG03.BusinessLogic.DTOs.Employees;
using DemoG03.BusinessLogic.Services.Interfaces;
using DemoG03.DataAccess.Models.Employees;
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
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public IEnumerable<EmployeeDto> GetAllEmployees(bool withTracking = false)
        {
            var employees = _employeeRepository.GetAll(withTracking);
            var employeesToReturn = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(employees);

            ///var employeesToReturn = employees.Select(E => new EmployeeDto
            ///{
            ///    Id = E.Id,
            ///    Name = E.Name,
            ///    Age = E.Age,
            ///    Email = E.Email,
            ///    EmployeeType = E.EmployeeType.ToString(),
            ///    Gender = E.Gender.ToString(),
            ///    IsActive = E.IsActive,
            ///    Salary = E.Salary
            ///});

            return employeesToReturn;
        }

        public EmployeeDetailsDto? GetEmployeeById(int id)
        {
            var employee = _employeeRepository.GetById(id);
            ////return employee is null ? null: new EmployeeDetailsDto()
            ////{
            ////    Id = employee.Id,
            ////    Name = employee.Name,
            ////    Age = employee.Age,
            ////    Email = employee.Email,
            ////    Salary = employee.Salary,
            ////    IsActive = employee.IsActive,
            ////    HiringDate = DateOnly.FromDateTime(employee.HiringDate),
            ////    Gender = employee.Gender.ToString(),
            ////    PhoneNumber = employee.PhoneNumber,
            ////    CreatedBy = employee.CreatedBy,
            ////    CreatedOn = employee.CreatedOn,
            ////    EmployeeType = employee.EmployeeType.ToString(),
            ////    LastModifiedBy = employee.LastModifiedBy,
            ////    LastModifiedOn = employee.LastModifiedOn
            ////};
            return employee is null ? null : _mapper.Map<Employee, EmployeeDetailsDto>(employee);
        }


        public int CreateEmployee(CreatedEmployeeDto emplyeeDto)
        {
            var employee = _mapper.Map<Employee>(emplyeeDto);
            return _employeeRepository.Add(employee);
        }
        public int UpdateEmployee(UpdatedEmployeeDto employeeDto)
        {
            return _employeeRepository.Update(_mapper.Map<Employee>(employeeDto));
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee is null) return false;

            #region Soft Delete
            employee.IsDeleted = true;
            var Result = _employeeRepository.Update(employee);
            return Result > 0 ? true : false; 
            #endregion

            ///Hard Delete
            ///if (employee is null) return false;
            ///var Result = _employeeRepository.Remove(employee);
            ///return Result > 0 ? true : false;

        }

    }
}
