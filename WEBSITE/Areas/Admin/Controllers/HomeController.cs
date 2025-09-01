using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WEBSITE.Models.ViewModels;
using WEBSITE.Data;

namespace WEBSITE.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var today = DateTime.Today;
			var yesterday = today.AddDays(-1);

			// Users
			var totalUser = await _context.Users.CountAsync();
			var todayUser = await _context.Users.CountAsync(u => u.CreateAt >= today);
			var yesterdayUser = await _context.Users.CountAsync(u => u.CreateAt >= yesterday && u.CreateAt < today);

			// Orders
			var totalOrder = await _context.Orders.CountAsync();
			var todayOrder = await _context.Orders.CountAsync(o => o.DateTime >= today);
			var yesterdayOrder = await _context.Orders.CountAsync(o => o.DateTime >= yesterday && o.DateTime < today);

			// Sales
			var totalSales = await _context.Orders.SumAsync(o => (decimal?)o.Total) ?? 0;
			var todaySales = await _context.Orders.Where(o => o.DateTime >= today).SumAsync(o => (decimal?)o.Total) ?? 0;
			var yesterdaySales = await _context.Orders.Where(o => o.DateTime >= yesterday && o.DateTime < today).SumAsync(o => (decimal?)o.Total) ?? 0;

			// Products
			var totalProducts = await _context.Products.CountAsync();

			// % change
			double userPercentChange = yesterdayUser > 0 ? ((double)(todayUser - yesterdayUser) / yesterdayUser) * 100 : 0;
			double orderPercentChange = yesterdayOrder > 0 ? ((double)(todayOrder - yesterdayOrder) / yesterdayOrder) * 100 : 0;
			double salesPercentChange = (double)yesterdaySales > 0 ? ((double)(todaySales - yesterdaySales) / (double)yesterdaySales) * 100 : 0;

			// Lưu vào bảng Statistics (nếu chưa có record cho ngày hôm nay thì thêm mới)
			var todayStatistic = await _context.Statistics.FirstOrDefaultAsync(s => s.Date == today);

			if (todayStatistic == null)
			{
				todayStatistic = new Models.Statistic
				{
					Date = today,
					TotalUsers = totalUser,
					TotalOrders = totalOrder,
					TotalProducts = totalProducts,
					TotalRevenue = totalSales
				};
				_context.Statistics.Add(todayStatistic);
			}
			else
			{
				todayStatistic.TotalUsers = totalUser;
				todayStatistic.TotalOrders = totalOrder;
				todayStatistic.TotalProducts = totalProducts;
				todayStatistic.TotalRevenue = totalSales;

				_context.Statistics.Update(todayStatistic);
			}

			await _context.SaveChangesAsync();

			// Truyền ra View nếu cần
			ViewBag.TotalUser = totalUser;
			ViewBag.TotalOrder = totalOrder;
			ViewBag.TotalSales = totalSales;
			ViewBag.TotalProducts = totalProducts;

			ViewBag.UserPercentChange = userPercentChange;
			ViewBag.OrderPercentChange = orderPercentChange;
			ViewBag.SalesPercentChange = salesPercentChange;

			return View();
		}

		// API: Trả dữ liệu chart (30 ngày gần nhất)
		[HttpGet]
		public async Task<IActionResult> GetSalesChart()
		{
			var raw = await _context.Statistics
				.Select(s => new { s.Date, s.TotalRevenue })
				.ToListAsync();

			var sales = raw.Select(s => new
			{
				Label = s.Date.ToString("dd/MM"),
				Value = s.TotalRevenue
			}).ToList();

			return Json(sales);
		}


		// API: Lấy danh sách categories
		[HttpGet]
		public IActionResult GetCategories()
		{
			var categories = _context.Categories
				.Select(c => new { c.CategoryId, c.Name })
				.ToList();

			return Json(categories);
		}
		public async Task<IActionResult> UserDetail()
		{
			var deal = await _context.Orders
				.Include(o => o.User)
				.Include(o => o.OrderDetals)
				.ThenInclude(od => od.Product)
				.Select(o => new DealViewModel
				{
					Fullname = o.User.Fullname ?? "",
					Location = o.User.Address ?? "",
					DateTime = o.DateTime,

					ProductName = o.OrderDetals.FirstOrDefault().Product.Name
								  ?? "Unknown Product",
					ProductImage = o.OrderDetals.FirstOrDefault().Product.ImageUrl
								   ?? "/Pictures/no-image.png",

					Piece = o.OrderDetals.FirstOrDefault().Quantity,
					Amount = o.Total,
					Status = o.Status ?? ""
				})
				.ToListAsync();

			return View(deal);
		}
	}
}