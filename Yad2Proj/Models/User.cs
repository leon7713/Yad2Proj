using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yad2Proj.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        [MaxLength(50, ErrorMessage = "FirstName must be 50 characters or less")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [MaxLength(50, ErrorMessage = "LastName must be 50 characters or less")]
        public string LastName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Required(ErrorMessage = "Please choose your birth date")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "The email address is not entred in a correct format")]
        [MaxLength(50, ErrorMessage = "Email must be 50 characters or less")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your user name")]
        [MaxLength(50, ErrorMessage = "UserName must be 50 characters or less")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [MaxLength(50, ErrorMessage = "Password must be 50 characters or less")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        public UserType UserType { get; set; }

        public virtual ICollection<Product> ProductsOwned { get; set; }
        public virtual ICollection<Product> ProductsBought { get; set; }
    }
    public enum UserType
    {
        Guest,
        Normal
    }
}
