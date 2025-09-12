using DemoG03.DataAccess.Data.Contexts;
using DemoG03.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG03.DataAccess.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public DepartmentRepository(ApplicationDbContext dbContext) // 1. Injection
                                                                    // Ask CLR forr Creating Application Obj [Dependency Injection]
        {
            _dbContext = dbContext;
        }
        //CRUD Operations
        public IEnumerable<Department> GetAll(bool WithTracking = false)
        {
            if (WithTracking)
            {
                return _dbContext.Departments.ToList();
            }
            else
            {
                return _dbContext.Departments.AsNoTracking().ToList();
            }
        }


        public Department? GetById(int id)
        {
            return _dbContext.Departments.Find(id);// Seearch First In Memory if theres nothing It Will go to database to Search
        }

        //Insert
        public int Add(Department department)
        {
            _dbContext.Departments.Add(department);
            return _dbContext.SaveChanges();
        }
        //Update
        public int Update(Department department)
        {
            _dbContext.Update(department);
            return _dbContext.SaveChanges();
        }
        //Remove
        public int Remove(Department department)
        {
            _dbContext.Remove(department);
            return _dbContext.SaveChanges();
        }
    }
}
