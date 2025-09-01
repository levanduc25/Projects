using System;

namespace WEBSITE.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public Category Category { get; set; }

        // Navigation properties
        public ICollection<OrderDetail> OrderDetals { get; set; }
        public ICollection<Review> Reviews { get; set; } 
        public ICollection<CartItem> CartItems { get; set; }
    }
}