using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEBSITE.Data;
using WEBSITE.Models;

namespace Website.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var category = _context.Categories.ToList();
            return View(category);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        public IActionResult Edit(int id)
        {
            var categoryId = _context.Categories.Find(id);
            if (categoryId == null)
            {
                return NotFound();
            }
            return View(categoryId);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        public IActionResult Delete(int id)
        {
            var categoryId = _context.Categories.Find(id);
            if (categoryId == null)
            {
                return NotFound();
            }
            return View(categoryId);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var categoryId = _context.Brands.Find(id);
            if (categoryId == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Brands.Remove(categoryId);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryId);
        }

    }
}