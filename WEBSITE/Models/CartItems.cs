using System;

namespace WEBSITE.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; } // Sửa từ CartItemrtId
        public int ProductId { get; set; }
        public int Quantity { get; set; } // Sửa từ Quality
        public decimal UnitPrice { get; set; }
        public Cart Cart { get; set; } // Sửa từ Carts
        public Product Product { get; set; } // Sửa từ Products
    }
}