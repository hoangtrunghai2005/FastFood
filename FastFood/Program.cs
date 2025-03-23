using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FastFood.Data;
using FastFood.Areas.Identity.Data; // Thay bằng namespace của bạn chứa FastFoodUser

var builder = WebApplication.CreateBuilder(args);

// Cấu hình DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình Identity với FastFoodUser
builder.Services.AddDefaultIdentity<FastFoodUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Không bắt buộc xác nhận email
    options.Password.RequireDigit = true; // Mật khẩu cần có số
    options.Password.RequiredLength = 6; // Độ dài tối thiểu
    options.Password.RequireUppercase = true; // Mật khẩu cần chữ in hoa
    options.Password.RequireLowercase = true; // Mật khẩu cần chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt buộc ký tự đặc biệt
})
.AddEntityFrameworkStores<ApplicationDbContext>() // Kết nối với ApplicationDbContext
.AddDefaultTokenProviders(); // Hỗ trợ token như reset mật khẩu

// Cấu hình Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn Session
    options.Cookie.HttpOnly = true; // Cookie chỉ truy cập qua HTTP
    options.Cookie.IsEssential = true; // Cookie cần thiết để Session hoạt động
});

// Thêm dịch vụ Razor Pages và MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Kích hoạt Middleware Session
app.UseSession();

app.UseAuthentication(); // Kích hoạt Authentication Middleware
app.UseAuthorization(); // Kích hoạt Authorization Middleware

// Định tuyến mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages(); // Hỗ trợ Razor Pages

app.Run();
