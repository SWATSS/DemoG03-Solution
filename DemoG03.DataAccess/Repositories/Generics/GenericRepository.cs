using DemoG03.DataAccess.Data.Contexts;
using DemoG03.DataAccess.Models;
using DemoG03.DataAccess.Models.Employees;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DemoG03.DataAccess.Repositories.Generics
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        //CRUD Operations
        public IEnumerable<TEntity> GetAll(bool WithTracking = false)
        {
            if (WithTracking)
            {
                return _dbContext.Set<TEntity>().Where(T => T.IsDeleted != true).ToList();
            }
            else
            {
                return _dbContext.Set<TEntity>().Where(T => T.IsDeleted != true).AsNoTracking().ToList();
            }
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return _dbContext.Set<TEntity>().Where(E => E.IsDeleted != true).Select(selector).ToList(); // Tolist (Encapsulation) so it will run here not outside the repo
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate).ToList();
        }


        public TEntity? GetById(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);// Search First In Memory if theres nothing It Will go to database to Search
        }

        //Insert
        public void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
        }
        //Update
        public void Update(TEntity entity)
        {
            _dbContext.Update(entity);
        }
        //Remove
        public void Remove(TEntity entity)
        {
            _dbContext.Remove(entity);
        }


    }
}
