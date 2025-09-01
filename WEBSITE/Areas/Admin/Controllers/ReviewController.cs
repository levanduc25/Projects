using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEBSITE.Data;
using WEBSITE.Areas;

namespace WEBSITE.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var reviews = _context.Reviews.ToList();
            return View(reviews);
        }
        public IActionResult Details(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }
        [HttpPost]
        public IActionResult ApproveorReject(int id, bool approve)
        {
            var review = _context.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }
            review.IsApproved = approve;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}