using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yad2Proj.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public User Owner { get; set; }
        public User User { get; set; }
        [MaxLength(50, ErrorMessage = "Title must be 50 characters or less")]
        public string Title { get; set; }
        [MaxLength(500, ErrorMessage = "Short description must be 500 characters or less")]
        public string ShortDesc { get; set; }
        [MaxLength(4000, ErrorMessage = "Long description must be 4000 characters or less")]
        public string LongDesc { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
        public decimal Price { get; set; }
        public byte[] Image1 { get; set; }
        public byte[] Image2 { get; set; }
        public byte[] Image3 { get; set; }
        public bool InCart { get; set; }
        public State State { get; set; }

    }
    public enum State
    {
        Available,
        Sold,
        Deleted
    }
}
