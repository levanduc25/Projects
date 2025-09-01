using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEBSITE.Data;

namespace WEBSITE.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StatisticController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StatisticController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var statistics = new
            {
                TotalReviews = _context.Reviews.Count(),
                ApprovedReviews = _context.Reviews.Count(r => r.IsApproved),
                RejectedReviews = _context.Reviews.Count(r => !r.IsApproved)
            };
            return View(statistics);
        }
    }
}