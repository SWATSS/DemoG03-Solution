using DemoG03.DataAccess.Models.Employees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG03.BusinessLogic.DTOs.Employees
{
    public class UpdatedEmployeeDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "XX")]
        [MaxLength(50, ErrorMessage = "Name Should Be Less Than 50 Chars")]
        [MinLength(3, ErrorMessage = "Name Should Be At Least 3 Chars")]
        public string Name { get; set; } = null!;
        [Range(18, 40)]
        public int? Age { get; set; }
        // 123-Street-City-Country
        [RegularExpression("^[1-9]{1,3}-[a-zA-Z]{5,10}-[a-z-A-Z]{5,10}-[a-z-A-Z]{5,10}$"
                                            , ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string? Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
        [Display(Name = "Hiring Date")]
        public DateOnly HiringDate { get; set; }
        public Gender Gender { get; set; }
        public EmployeeType EmployeeType { get; set; }
    }
}
