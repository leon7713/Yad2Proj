using System;
using System.Linq;
using System.Linq.Expressions;

namespace Yad2Proj.Data.Repository
{
    public interface IRepositoryOf<TKey, TEntity>
       where TEntity : class, new()
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById(TKey id);
        IQueryable<TEntity> GetByIdJoin(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] children);
        void Update(TKey id, TEntity Entity);
        bool Delete(TKey id);
        TEntity Create(TEntity Entity);
    }
}
