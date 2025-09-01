using System.Collections.Generic;

namespace WEBSITE.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation properties
        public ICollection<Product> Products { get; set; }
        public ICollection<Brand> Brands { get; set; }
    }
}