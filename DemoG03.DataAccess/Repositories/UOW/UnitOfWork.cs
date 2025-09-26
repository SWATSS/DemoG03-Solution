using DemoG03.DataAccess.Data.Contexts;
using DemoG03.DataAccess.Repositories.Departments;
using DemoG03.DataAccess.Repositories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG03.DataAccess.Repositories.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private Lazy<IDepartmentRepository> _departmentRepository;
        private Lazy<IEmployeeRepository> _employeeRepository;
        private ApplicationDbContext _dbContex;

        public UnitOfWork(ApplicationDbContext dbContex)
        {
            _dbContex = dbContex;
            _departmentRepository = new Lazy<IDepartmentRepository>(() => new DepartmentRepository(_dbContex));
            _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(_dbContex));
        }

        public IEmployeeRepository EmployeeRepository => _employeeRepository.Value;

        public IDepartmentRepository DepartmentRepository => _departmentRepository.Value;

        public int SaveChanges()
        {
            return _dbContex.SaveChanges();
        }
    }
}
