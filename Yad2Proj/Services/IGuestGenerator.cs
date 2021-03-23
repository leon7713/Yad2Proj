using Yad2Proj.Data.Repository;
using Yad2Proj.Models;

namespace Yad2Proj.Services
{
    public interface IGuestGenerator
    {
        public User GenUser(IRepositoryOf<int, User> usersContext);

    }
}
