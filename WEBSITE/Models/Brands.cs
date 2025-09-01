using System;

namespace WEBSITE.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
    }
}