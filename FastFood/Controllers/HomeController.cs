using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FastFood.Data;
using FastFood.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FastFood.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Trang chính
        public IActionResult Index(string? tuKhoa = null)
        {
            // Lấy danh sách món ăn từ cơ sở dữ liệu
            var danhSachMonAn = _context.DoAnNhanhs.AsQueryable();

            // Nếu có từ khóa tìm kiếm, lọc dữ liệu
            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                danhSachMonAn = danhSachMonAn.Where(x => x.MonAn.Contains(tuKhoa) || x.MoTa.Contains(tuKhoa));
            }

            // Truyền từ khóa và danh sách món ăn qua ViewData
            ViewBag.TuKhoa = tuKhoa; // Hiển thị từ khóa trong View
            ViewData["DanhSachMonAn"] = danhSachMonAn.ToList();

            return View();
        }

        // Xử lý Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public IActionResult ThemVaoGio(int id)
        {
            // Lấy giỏ hàng từ Session
            var gioHangJson = HttpContext.Session.GetString("GioHang");
            var gioHang = string.IsNullOrEmpty(gioHangJson)
                ? new List<ChiTietDonHang>() // Nếu giỏ hàng trống, tạo danh sách mới
                : JsonConvert.DeserializeObject<List<ChiTietDonHang>>(gioHangJson) ?? new List<ChiTietDonHang>();

            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var sanPham = gioHang.FirstOrDefault(x => x.DoAnNhanhID == id);
            if (sanPham != null)
            {
                sanPham.SoLuong += 1; // Tăng số lượng sản phẩm
            }
            else
            {
                gioHang.Add(new ChiTietDonHang
                {
                    DoAnNhanhID = id,
                    SoLuong = 1
                });
            }

            // Cập nhật giỏ hàng lại vào Session
            HttpContext.Session.SetString("GioHang", JsonConvert.SerializeObject(gioHang));

            // Trả về phản hồi JSON
            return Json(new { success = true });
        }

        // Trang Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // Xử lý lỗi
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
