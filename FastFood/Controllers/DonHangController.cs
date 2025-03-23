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
            var donHangs = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(c => c.DoAnNhanh)
                .ToListAsync();
            return View(donHangs);
        }

        // Xem chi tiết đơn hàng
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
    }
}
