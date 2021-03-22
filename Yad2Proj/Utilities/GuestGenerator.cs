using System;
using System.Collections.Generic;
using System.Linq;
using Yad2Proj.Data;
using Yad2Proj.Models;

namespace Yad2Proj.Utilities
{
    public class GuestGenerator : IGuestGenerator
    {
        
        
        public GuestGenerator()
        {
        }
        public User GenUser(IRepositoryOf<int,User> usersContext)
        {

            User user = new User()
            {
                UserName = GetValidUserName(usersContext.GetAll()),
                FirstName = "guest",
                LastName = "guest",
                Email = "",
                BirthDate = DateTime.Now,
                Password = "",
                UserType = UserType.Guest
            };
            return usersContext.Create(user);
        }

        private string GetValidUserName(IQueryable<User> currentUsers)
        {
            List<User> guests = currentUsers.Where(x => x.UserName.Contains("guest")).ToList<User>();
            int index = 1;
            string userName;
            do
            {
                userName = "guest" + index++;
            } while (guests.Where(x => x.UserName == userName).Count() > 0);

            return userName;
        }
    }
}
