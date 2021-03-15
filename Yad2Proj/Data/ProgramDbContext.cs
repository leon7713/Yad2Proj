using Microsoft.EntityFrameworkCore;
using Yad2Proj.Models;

namespace Yad2Proj.Data
{
    public class ProgramDbContext : DbContext
    {
        public ProgramDbContext()
        {
            
        }
        public ProgramDbContext(DbContextOptions<ProgramDbContext> opts) : base(opts)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>().Property(x => x.Price).HasPrecision(18, 2);
            builder.Entity<User>().HasMany(x => x.ProductsOwned).WithOne(con => con.Owner);
            builder.Entity<User>().HasMany(x => x.ProductsBought).WithOne(con => con.User);

            base.OnModelCreating(builder);
        }
    }
}
