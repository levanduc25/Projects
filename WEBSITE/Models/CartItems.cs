using System;

namespace WEBSITE.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartItemrtId { get; set; }
        public int ProductId { get; set; }
        public int Quality { get; set;}
        public decimal UnitPrice { get; set; }
        public Cart Carts { get; set; }
        public Product Products { get; set; }

    }
}