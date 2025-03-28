using Microsoft.AspNetCore.Mvc;
using FastFood.Data;
using FastFood.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Controllers
{
    public class DonHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách đơn hàng
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách đơn hàng kèm chi tiết
            var donHangs = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(c => c.DoAnNhanh)
                .ToListAsync();
            return View(donHangs);
        }

        // Xem chi tiết một đơn hàng
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy đơn hàng theo ID
            var donHang = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(c => c.DoAnNhanh)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // Xử lý thanh toán với mã QR tĩnh
        [HttpPost]
        public JsonResult LayMaQrTinh()
        {
            // Trả về mã QR tĩnh và thông báo thành công
            return Json(new
            {
                success = true,
                qrCodeUrl = "https://qr.sepay.vn/img?acc=96247HTH96247&bank=BIDV", // URL mã QR tĩnh
                message = "Vui lòng quét mã QR để thanh toán."
            });
        }
    }
}
