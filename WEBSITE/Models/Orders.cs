using System;

namespace WEBSITE.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }

        public User User;
        
        //Connect to OrderDetail
        public ICollection<OrderDetail> OrderDetals { get; set; }

    }
}