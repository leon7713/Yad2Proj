using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yad2Proj.Models;

namespace Yad2Proj.Data
{
    public interface IRepositoryOf<TKey, TEntity>
        where TEntity : class, new()
    {
        IEnumerable<TEntity> ShowAll();
        TEntity GetById(TKey id);
        void Update(TKey id, TEntity Entity);
        void Delete(TKey id);
        void Create(TEntity Entity);

    }
}
