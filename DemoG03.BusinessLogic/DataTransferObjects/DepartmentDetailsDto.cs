using DemoG03.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG03.BusinessLogic.DataTransferObjects
{
    public class DepartmentDetailsDto
    {
        #region CTOR Mapping
        //public DepartmentDetailsDto(Department department)
        //{
        //    Id = department.Id;
        //    Name = department.Name;
        //    Code = department.Code;
        //    Description = department.Description;
        //    CreatedBy = department.CreatedBy;
        //    DateOfCreation = DateOnly.FromDateTime(department.CreatedOn ?? DateTime.Now),;
        //    IsDeleted = department.IsDeleted;
        //} 
        #endregion
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Description { get; set; } = string.Empty!;
        public int CreatedBy { get; set; }
        public DateOnly? DateOfCreation { get; set; }
        public int LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
