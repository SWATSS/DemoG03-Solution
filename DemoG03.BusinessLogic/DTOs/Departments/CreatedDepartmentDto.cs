using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG03.BusinessLogic.DTOs.Departments
{
    public class CreatedDepartmentDto
    {
        public string Name { get; set; } = null!;
        [Range(10, int.MaxValue)]
        public string Code { get; set; } = null!;
        public string Description { get; set; } = string.Empty!;
        public DateOnly DateOfCreation { get; set; }

    }
}
