using System;

namespace WEBSITE.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; } 
        public string Comment { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; }
        public bool IsApproved { get; set; } = false;

        // Thêm navigation properties
        public User User { get; set; }
        public Product Product { get; set; }
    }
}