using DemoG03.DataAccess.Data.Contexts;
using DemoG03.DataAccess.Models.Departments;
using DemoG03.DataAccess.Repositories.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG03.DataAccess.Repositories.Departments
{
    public class DepartmentRepository : GenericRepository<Department>,IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext) // 1. Injection
                                                                                      // Ask CLR forr Creating Application Obj [Dependency Injection]
        {
            _dbContext = dbContext;
        }
        
    }
}
