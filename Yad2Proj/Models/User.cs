using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Yad2Proj.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "FirstName must be 50 characters or less")]
        public string FirstName { get; set; }
        [MaxLength(50, ErrorMessage = "LastName must be 50 characters or less")]
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        [MaxLength(50, ErrorMessage = "Email must be 50 characters or less")]
        public string Email { get; set; }
        [MaxLength(50, ErrorMessage = "UserName must be 50 characters or less")]
        public string UserName { get; set; }
        [MaxLength(50, ErrorMessage = "Password must be 50 characters or less")]
        public string Password { get; set; }
        public virtual ICollection<Product> ProductsOwned { get; set; }
        public virtual ICollection<Product> ProductsBought { get; set; }
    }
}
