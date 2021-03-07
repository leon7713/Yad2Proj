using Microsoft.EntityFrameworkCore;

namespace Yad2Proj.Data
{
    public interface IProgramDbContextProvider
    {
        DbContext Get();
    }
    public class DbContextProviderOf<TDbContext> : IProgramDbContextProvider
        where TDbContext : DbContext, new()
    {
        public DbContext Get()
        {
            return new TDbContext;
        }
    }
}
