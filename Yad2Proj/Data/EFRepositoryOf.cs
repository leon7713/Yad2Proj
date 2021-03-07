using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yad2Proj.Models;

namespace Yad2Proj.Data
{
    public class EFRepositoryOf<TKey , TEntity> : IRepositoryOf<TKey, TEntity> 
        where TEntity : class, new()
    {
        private readonly IDbContextProvider _dbContextProvider;
        public EFRepositoryOf(IDbContextProvider dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }
        public void Create(TEntity entity)
        {
            using (var db = _dbContextProvider.GetDbContext())
            {
                DbSet<TEntity> dbSet = db.Set<TEntity>();
                dbSet.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(TKey id)
        {
            using (var db = _dbContextProvider.GetDbContext())
            {
                DbSet<TEntity> dbSet = db.Set<TEntity>();
                var foundEntity = db.Find<TEntity>(id);
                if (foundEntity != null)
                {
                    dbSet.Remove(foundEntity);
                }
                db.SaveChanges();
            }
        }

        public TEntity GetById(TKey id)
        {
            using (var db = _dbContextProvider.GetDbContext())
            {
                DbSet<TEntity> dbSet = db.Set<TEntity>();
                var entity = dbSet.Find(id);
                return entity;
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            using (var db = _dbContextProvider.GetDbContext())
            {
                DbSet<TEntity> dbSet = db.Set<TEntity>();
                return dbSet.ToList<TEntity>();
            }
        }

        public void Update(TKey id, TEntity newEntity)
        {
            using (var db = _dbContextProvider.GetDbContext())
            {
                DbSet<TEntity> dbSet = db.Set<TEntity>();
                var oldEntity = dbSet.Find(id);
                oldEntity = newEntity;
                db.SaveChanges();
            }
        }
    }
}
