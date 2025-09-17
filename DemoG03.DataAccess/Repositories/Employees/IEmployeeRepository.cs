using DemoG03.DataAccess.Models.Employees;
using DemoG03.DataAccess.Repositories.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG03.DataAccess.Repositories.Employees
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
    }
}
