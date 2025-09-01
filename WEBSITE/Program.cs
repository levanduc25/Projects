using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using WEBSITE.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddAuthentication(options =>
{
    // mặc định challenge khi gặp [Authorize]
    options.DefaultScheme = "Admin"; 
    options.DefaultChallengeScheme = "Admin"; 
    options.DefaultSignInScheme = "Admin";
})
.AddCookie("User", options =>
{
    options.LoginPath = "/User/Account/Login";
    options.LogoutPath = "/User/Account/Logout";
    options.AccessDeniedPath = "/User/Account/AccessDenied";
})
.AddCookie("Admin", options =>
{
    options.LoginPath = "/Admin/Account/Login";
    options.LogoutPath = "/Admin/Account/Logout";
    options.AccessDeniedPath = "/Admin/Account/AccessDenied";
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Route cho khu vực Admin
app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}",
    constraints: new { area = "Admin" });

// Route cho khu vực User
app.MapControllerRoute(
    name: "user",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}",
    constraints: new { area = "User" });

// Route mặc định (có thể dẫn đến trang login của user)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
