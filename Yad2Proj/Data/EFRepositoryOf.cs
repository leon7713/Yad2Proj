using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Yad2Proj.Data
{
    public class EFRepositoryOf<TKey, TEntity> : IRepositoryOf<TKey, TEntity>
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
        public void Create(TEntity entity)
        {

            _dbSet.Add(entity);
            _context.SaveChanges();

        }

        public void Delete(TKey id)
        {
            var db = _dbContextProvider.GetDbContext();

            DbSet<TEntity> dbSet = db.Set<TEntity>();
            var foundEntity = db.Find<TEntity>(id);
            if (foundEntity != null)
            {
                dbSet.Remove(foundEntity);
            }
            db.SaveChanges();

        }

        public TEntity GetById(TKey id)
        {
            var entity = _dbSet.Find(id);
            return entity;
        }

        public IEnumerable<TEntity> GetAll()
        {

            return _dbSet.ToList<TEntity>();

        }

        public void Update(TKey id, TEntity newEntity)
        {
            _dbSet.Attach(newEntity);
            _context.Entry(newEntity).State = EntityState.Modified;
        }

        //public TEntity GetByUsernameAndPassword(string username, string password)
        //{
        //   TEntity user;

        //   using (var db = _dbContextProvider.GetDbContext())
        //   {
        //      DbSet<TEntity> dbSet = db.Set<TEntity>();
        //      user = dbSet.Local.SingleOrDefault(u => u)
        //   }
        //   return user;
        //}
    }
}
