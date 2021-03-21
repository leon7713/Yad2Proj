using Yad2Proj.Data;
using Yad2Proj.Models;

namespace Yad2Proj.Utilities
{
    public interface IGuestGenerator
    {
        public User GenUser(IRepositoryOf<int, User> usersContext);

    }
}
