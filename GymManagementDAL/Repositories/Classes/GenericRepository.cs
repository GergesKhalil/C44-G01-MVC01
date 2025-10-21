﻿using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        private readonly GymDbContext _dbContext;

        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
            
        
        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if(condition is not null)
                return _dbContext.Set<TEntity>().AsNoTracking().Where(condition).ToList();
            else
                return _dbContext.Set<TEntity>().AsNoTracking().ToList();

        }
        public void Add(TEntity entity) => _dbContext.Add(entity);
        public void Delete(TEntity entity) =>  _dbContext.Remove(entity);
        public void Update(TEntity entity) => _dbContext.Update(entity);

        public TEntity? GetById(int id) => _dbContext.Set<TEntity>().Find(id);

       
        
        

        

       
        
    }
}
