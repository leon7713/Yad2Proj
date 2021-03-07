using Microsoft.EntityFrameworkCore;

namespace Yad2Proj.Data
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
