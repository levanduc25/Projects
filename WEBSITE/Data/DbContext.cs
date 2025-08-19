using Microsoft.EntityFrameworkCore;
using WEBSITE.Models;

namespace WEBSITE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        // User related
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        // Order related
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        // Product related
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Promotion> Promotions { get; set; }

        // Others
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

    }
}
