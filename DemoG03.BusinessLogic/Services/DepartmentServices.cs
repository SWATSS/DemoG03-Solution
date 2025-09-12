using DemoG03.BusinessLogic.DataTransferObjects;
using DemoG03.BusinessLogic.Factories;
using DemoG03.DataAccess.Models;
using DemoG03.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG03.BusinessLogic.Services
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentServices(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public IEnumerable<DepartmentDto> GetAllDepartments()
        {
            var departments = _departmentRepository.GetAll();
            #region Old Way
            //var departmentsToReturn = departments.Select(D => new DepartmentDto
            //{
            //    Id = D.Id,
            //    Name = D.Name,
            //    Code = D.Code,
            //    Description = D.Description,
            //    DateOfCreation = DateOnly.FromDateTime(D.CreatedOn ?? DateTime.Now)
            //}); 
            #endregion
            var departmentsToReturn = departments.Select(D => D.ToDepartmentDto());
            return departmentsToReturn;
        }

        public DepartmentDetailsDto? GetDepartmentById(int id)
        {
            var department = _departmentRepository.GetById(id);
            #region Long Way
            //if (department is null)
            //{
            //    return null;
            //}
            //else
            //{
            //    return new DepartmentDetailsDto()
            //    {
            //        Id = department.Id,
            //        Name = department.Name,
            //        Code = department.Code,
            //        Description = department.Description,
            //        CreatedBy = department.CreatedBy,
            //        DateOfCreation = DateOnly.FromDateTime(department.CreatedOn ?? DateTime.Now),
            //        IsDeleted = department.IsDeleted
            //    };
            //} 
            #endregion
            #region Manual Mapping
            // Manual Mapping
            //return department == null ? null : new DepartmentDetailsDto()
            //{
            //    Id = department.Id,
            //    Name = department.Name,
            //    Code = department.Code,
            //    Description = department.Description,
            //    CreatedBy = department.CreatedBy,
            //    DateOfCreation = DateOnly.FromDateTime(department.CreatedOn ?? DateTime.Now),
            //    IsDeleted = department.IsDeleted
            //}; 
            #endregion
            // AutoMapper
            #region Contstructor Mapping
            // Contstructor Mapping
            //return department == null ? null : new DepartmentDetailsDto(department); 
            #endregion
            //Extension Method (DepartmentFactory)
            return department == null ? null : department.ToDepartmentDetailsDto();

        }

        public int AddDepartment(CreatedDepartmentDto departmentDto)
        {
            return _departmentRepository.Add(departmentDto.ToEntity());
        }
        public int UpdateDepartment(UpdatedDepartmentDto departmentDto)
        {
            return _departmentRepository.Update(departmentDto.ToEntity());
        }

        public bool DeleteDepartment(int id)
        {
            var department = _departmentRepository.GetById(id);
            if (department == null) return false;
            else
            {
                var result = _departmentRepository.Remove(department);
                return result > 0 ? true : false;
            }
        }

    }
}
