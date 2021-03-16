using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Yad2Proj.Data
{
    public interface IRepositoryOf<TKey, TEntity>
       where TEntity : class, new()
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(TKey id);
        IQueryable<TEntity> GetByIdJoin(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] children);
        void Update(TKey id, TEntity Entity);
        void Delete(TKey id);
        void Create(TEntity Entity);
        //TEntity GetByUsernameAndPassword(string username, string password);
    }
}
