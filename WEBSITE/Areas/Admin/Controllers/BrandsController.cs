using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEBSITE.Data;
using WEBSITE.Models;

namespace Website.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class BrandsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrandsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var brands = _context.Brands.ToList();
            return View(brands);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            if (ModelState.IsValid)
            {
                _context.Brands.Add(brand);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }
        public IActionResult Edit(int id)
        {
            var brandId = _context.Brands.Find(id);
            if (brandId == null)
            {
                return NotFound();
            }
            return View(brandId);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Brand brand)
        {
            if (ModelState.IsValid)
            {
                _context.Brands.Update(brand);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }
        public IActionResult Delete(int id)
        {
            var brandId = _context.Brands.Find(id);
            if (brandId == null)
            {
                return NotFound();
            }
            return View(brandId);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var brandId = _context.Brands.Find(id);
            if (brandId == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Brands.Remove(brandId);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(brandId);
        }

    }
}