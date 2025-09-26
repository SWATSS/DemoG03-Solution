using AutoMapper;
using DemoG03.BusinessLogic.DTOs.Employees;
using DemoG03.BusinessLogic.Services.Interfaces;
using DemoG03.DataAccess.Data.Migrations;
using DemoG03.DataAccess.Models.Employees;
using DemoG03.DataAccess.Repositories.Employees;
using DemoG03.DataAccess.Repositories.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG03.BusinessLogic.Services.Classes
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public EmployeeService(IUnitOfWork unitOfWork
                             , IMapper mapper
                             , IAttachmentService attachmentService)
        {
            _mapper = mapper;
            _attachmentService = attachmentService;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeDto> GetAllEmployees(string? EmployeeSearchName, bool withTracking = false)
        {
            //var employees = _employeeRepository.GetAll(withTracking);
            //.Where(E => E.Name.ToLower().Contains(EmployeeSearchName.ToLower()));
            IEnumerable<Employee> employees;
            if (string.IsNullOrWhiteSpace(EmployeeSearchName))
            {
                employees = _unitOfWork.EmployeeRepository.GetAll(withTracking);
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.GetAll(E => E.Name.ToLower().Contains(EmployeeSearchName.ToLower()));
            }

            var employeesToReturn = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(employees);

            ///var employees = _employeeRepository.GetAll(E => new EmployeeDto()
            ///{
            ///    Id = E.Id,
            ///    Name = E.Name,
            ///    Age = E.Age,
            ///    Email = E.Email,
            ///    Salary = E.Salary
            ///});
            ///}).Where(E => E.Age > 24); It Will Proccessed after BC (Encapsulation)


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
            var employee = _unitOfWork.EmployeeRepository.GetById(id);
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


        public int CreateEmployee(CreatedEmployeeDto employeeDto)
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            if (employeeDto.Image is not null)
            {
                employee.ImageName = _attachmentService.Upload(employeeDto.Image, "Images");
            }
            _unitOfWork.EmployeeRepository.Add(employee);
            return _unitOfWork.SaveChanges();
        }
        public int UpdateEmployee(UpdatedEmployeeDto employeeDto)
        {
            var oldEmployee = _unitOfWork.EmployeeRepository.GetById(employeeDto.Id);
            var oldImageName = oldEmployee.ImageName;
            if (!string.IsNullOrEmpty(oldImageName) && employeeDto.Image is not null)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", "Images");

                _attachmentService.Delete(Path.Combine(folderPath, oldImageName));
            }
            // Fix For => [The instance of entity type 'Employee' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached. Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values.]
            _unitOfWork.Detach(oldEmployee);

            var employee = _mapper.Map<Employee>(employeeDto);
            if (employeeDto.Image is not null)
            {
                employee.ImageName = _attachmentService.Upload(employeeDto.Image, "Images");
            }
            _unitOfWork.EmployeeRepository.Update(employee);
            return _unitOfWork.SaveChanges();
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _unitOfWork.EmployeeRepository.GetById(id);
            if (employee is null) return false;

            #region Soft Delete
            employee.IsDeleted = true;
            _unitOfWork.EmployeeRepository.Update(employee);
            var Result = _unitOfWork.SaveChanges();
            return Result > 0 ? true : false;
            #endregion

            ///Hard Delete
            ///if (employee is null) return false;
            ///var Result = _employeeRepository.Remove(employee);
            ///return Result > 0 ? true : false;

        }

    }
}
