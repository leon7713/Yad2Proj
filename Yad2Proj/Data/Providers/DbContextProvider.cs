

using Yad2Proj.Data.Context;

namespace Yad2Proj.Data.Providers
{
    public class DbContextProvider : IDbContextProvider
    {
        ProgramDbContext _context;
        public DbContextProvider(ProgramDbContext context)
        {
            _context = context;
        }
        public ProgramDbContext GetDbContext()
        {
            return _context;
        }
    }
}
