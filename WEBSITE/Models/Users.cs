using System;

namespace WEBSITE.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime Date { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string Role { get; set; } = "User"; 

        // Navigation properties
        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }
        public Cart Cart { get; set; } 
    }
}