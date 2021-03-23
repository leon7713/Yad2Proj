using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Yad2Proj.Data.Context;
using Yad2Proj.Data.Providers;

namespace Yad2Proj.Data.Repository
{
   public class EFRepositoryOf<TKey, TEntity> : IRepositoryOf<TKey, TEntity>, IDisposable
      where TEntity : class, new()
   {
      private readonly IDbContextProvider _dbContextProvider;
      private readonly ProgramDbContext _context;
      internal DbSet<TEntity> _dbSet;
      public EFRepositoryOf(IDbContextProvider dbContextProvider)
      {
         _dbContextProvider = dbContextProvider;
         _context = dbContextProvider.GetDbContext();
         _dbSet = _context.Set<TEntity>();
      }
      public TEntity Create(TEntity entity)
      {

         _dbSet.Add(entity);
         _context.SaveChanges();
            return entity;
      }

      public bool Delete(TKey id)
      {
         var db = _dbContextProvider.GetDbContext();

         DbSet<TEntity> dbSet = db.Set<TEntity>();
         var foundEntity = db.Find<TEntity>(id);
         if (foundEntity != null)
         {
            dbSet.Remove(foundEntity);
         }
         int affected = db.SaveChanges();
            return affected > 0;
      }

      public TEntity GetById(TKey id)
      {
         var entity = _dbSet.Find(id);
         return entity;
      }
      public virtual IQueryable<TEntity> GetByIdJoin(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] children)
      {
         children.ToList().ForEach(x => _dbSet.Include(x).Load());
         return _dbSet.Where(filter);
      }

      public IQueryable<TEntity> GetAll()
      {
         return _dbSet;
      }

      public void Update(TKey id, TEntity newEntity)
      {
         var entity = _dbSet.Attach(newEntity);
         _context.Entry(newEntity).State = EntityState.Modified;
         _context.SaveChanges();
      }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
