using System;

namespace WEBSITE.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }  // <- EF Core sẽ nhận là PK

        // FK to Orders
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // FK to Product
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}