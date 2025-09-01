using System;

namespace WEBSITE.Models
{
    public class Promotion
    {
        public int PromotionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public ICollection <Product> Products{ get; set; }
    }
}