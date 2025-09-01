using System;

namespace WEBSITE.Models.ViewModels
{
    public class DealViewModel
    {
        public string Fullname { get; set; } = "";
        public string ProductName { get; set; } = "/wwwroot/Pictures/no-image.png";
        public string ProductImage { get; set; } = "unkhown";
        public string Location { get; set; } = "";
        public DateTime DateTime { get; set; }
        public int Piece { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "";
    }
}