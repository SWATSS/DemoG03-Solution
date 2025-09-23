using DemoG03.BusinessLogic.DTOs.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG03.BusinessLogic.Services.Interfaces
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetAllEmployees(string EmployeeSearchName, bool withTracking = false);
        EmployeeDetailsDto? GetEmployeeById(int id);
        int CreateEmployee(CreatedEmployeeDto emplyeeDto);
        int UpdateEmployee(UpdatedEmployeeDto employeeDto);
        bool DeleteEmployee(int id);
    }
}
