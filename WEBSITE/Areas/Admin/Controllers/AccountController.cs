using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WEBSITE.Data;
using WEBSITE.Models.ViewModels;
using WEBSITE.Models;
using Microsoft.AspNetCore.Identity;

namespace WEBSITE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        private const int MaxLoginAttempts = 5;
        private const int LockoutMinutes = 30;

        public AccountController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Find user
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found");
                    return View(model);
                }

                // Verify password
                var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
                if (verifyResult == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError(string.Empty, "Invalid password");
                    return View(model);
                }

                // Must be Admin
                if (user.Role != "Admin")
                {
                    
                    return RedirectToAction("AccessDenied", "Account", new { area = "Admin" });
                }

                // Claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("Fullname", user.Fullname ?? string.Empty),
                    new Claim("LoginTime", DateTime.UtcNow.ToString("o"))
                };

                // Identity với scheme "Admin"
                var claimsIdentity = new ClaimsIdentity(claims, "Admin");

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe
                        ? DateTimeOffset.UtcNow.AddDays(7)
                        : DateTimeOffset.UtcNow.AddMinutes(30),
                    AllowRefresh = true,
                    IssuedUtc = DateTimeOffset.UtcNow
                };

                // Đăng nhập với cookie "Admin"
                await HttpContext.SignInAsync("Admin",
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login Error: {ex.Message}");
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View(model);
            }

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(model);
            }

            var user = new User
            {
                Fullname = model.FullName,
                Email = model.Email,
                Username = model.Username,
                Phone = model.Phone,
                Address = model.Address,
                Date = model.Date,
                CreateAt = DateTime.UtcNow,
                Role = "User" 
            };

            // Hash password
            user.Password = _passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Admin");
            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
