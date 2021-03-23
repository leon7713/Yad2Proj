using Yad2Proj.Data.Context;

namespace Yad2Proj.Data.Providers
{
    public interface IDbContextProvider
    {
        ProgramDbContext GetDbContext();
    }
}
