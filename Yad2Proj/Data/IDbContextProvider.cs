namespace Yad2Proj.Data
{
    public interface IDbContextProvider
    {
        ProgramDbContext GetDbContext();
    }
}
