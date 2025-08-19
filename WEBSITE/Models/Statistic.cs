using System;

namespace WEBSITE.Models
{
    public class Statistic
    {
        public int StatisticId { get; set; }   

        public DateTime Date { get; set; }     
        public int TotalUsers { get; set; }      
        public int TotalOrders { get; set; }       
        public int TotalProducts { get; set; }     
        public decimal TotalRevenue { get; set; } 
    }
}
