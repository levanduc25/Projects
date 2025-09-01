using System;

namespace WEBSITE.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; } // Đổi từ string sang int
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }

        public User User { get; set; }
        
        //Connect to OrderDetail
        public ICollection<OrderDetail> OrderDetals { get; set; }
    }
}