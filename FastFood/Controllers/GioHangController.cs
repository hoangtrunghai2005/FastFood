using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using FastFood.Data;
using FastFood.Models;
using System.Collections.Generic;
using System.Linq;

namespace FastFood.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GioHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị giỏ hàng
        public IActionResult Index()
        {
            var gioHang = GetGioHang();
            foreach (var item in gioHang)
            {
                // Lấy thông tin sản phẩm từ cơ sở dữ liệu
                item.DoAnNhanh = _context.DoAnNhanhs.FirstOrDefault(d => d.ID == item.DoAnNhanhID);
                if (item.DoAnNhanh == null)
                {
                    item.DoAnNhanh = new DoAnNhanh
                    {
                        MonAn = "Sản phẩm không tồn tại",
                        Gia = 0
                    };
                }
            }

            // Chuẩn bị một đối tượng DonHang để nhập thông tin thanh toán
            ViewData["DonHang"] = new DonHang();
            return View(gioHang);
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public IActionResult ThemSanPham(int doAnNhanhId, int soLuong)
        {
            var sanPham = _context.DoAnNhanhs.FirstOrDefault(d => d.ID == doAnNhanhId);
            if (sanPham == null)
            {
                TempData["Error"] = "Sản phẩm không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            var gioHang = GetGioHang();
            var chiTiet = gioHang.FirstOrDefault(c => c.DoAnNhanhID == doAnNhanhId);
            if (chiTiet != null)
            {
                chiTiet.SoLuong += soLuong; // Cộng thêm số lượng
            }
            else
            {
                gioHang.Add(new ChiTietDonHang
                {
                    DoAnNhanhID = doAnNhanhId,
                    SoLuong = soLuong
                });
            }

            SaveGioHang(gioHang);
            TempData["Success"] = "Sản phẩm đã được thêm vào giỏ hàng.";
            return RedirectToAction(nameof(Index));
        }

        // Giảm số lượng sản phẩm trong giỏ hàng
        public IActionResult GiamSoLuong(int doAnNhanhId)
        {
            var gioHang = GetGioHang();
            var chiTiet = gioHang.FirstOrDefault(c => c.DoAnNhanhID == doAnNhanhId);

            if (chiTiet != null)
            {
                if (chiTiet.SoLuong > 1)
                {
                    chiTiet.SoLuong--;
                    TempData["Success"] = "Đã giảm 1 sản phẩm.";
                }
                else
                {
                    gioHang.Remove(chiTiet);
                    TempData["Success"] = "Sản phẩm đã được xóa khỏi giỏ hàng.";
                }

                SaveGioHang(gioHang);
            }
            else
            {
                TempData["Error"] = "Không tìm thấy sản phẩm trong giỏ hàng.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Tăng số lượng sản phẩm trong giỏ hàng
        public IActionResult TangSoLuong(int doAnNhanhId)
        {
            var gioHang = GetGioHang();
            var chiTiet = gioHang.FirstOrDefault(c => c.DoAnNhanhID == doAnNhanhId);

            if (chiTiet != null)
            {
                chiTiet.SoLuong++;
                SaveGioHang(gioHang);
                TempData["Success"] = "Đã tăng 1 sản phẩm.";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy sản phẩm trong giỏ hàng.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Xóa hoàn toàn sản phẩm khỏi giỏ hàng
        public IActionResult XoaSanPham(int doAnNhanhId)
        {
            var gioHang = GetGioHang();
            var chiTiet = gioHang.FirstOrDefault(c => c.DoAnNhanhID == doAnNhanhId);

            if (chiTiet != null)
            {
                gioHang.Remove(chiTiet);
                SaveGioHang(gioHang);
                TempData["Success"] = "Sản phẩm đã được xóa hoàn toàn khỏi giỏ hàng.";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy sản phẩm trong giỏ hàng.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Thanh toán giỏ hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThanhToan(string tenKhachHang, string diaChi, string soDienThoai, string phuongThucThanhToan)
        {
            var gioHang = GetGioHang();
            if (gioHang == null || !gioHang.Any())
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống. Vui lòng thêm sản phẩm.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(tenKhachHang) || string.IsNullOrWhiteSpace(diaChi) ||
                string.IsNullOrWhiteSpace(soDienThoai) || string.IsNullOrWhiteSpace(phuongThucThanhToan))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin để thanh toán.";
                return RedirectToAction(nameof(Index));
            }

            // Lưu thông tin đơn hàng
            var donHang = new DonHang
            {
                TenKhachHang = tenKhachHang,
                DiaChi = diaChi,
                SoDienThoai = soDienThoai,
                PhuongThucThanhToan = phuongThucThanhToan,
                NgayDat = System.DateTime.Now
            };
            _context.DonHangs.Add(donHang);
            _context.SaveChanges();

            // Lưu từng chi tiết sản phẩm trong giỏ hàng vào cơ sở dữ liệu
            foreach (var item in gioHang)
            {
                var chiTiet = new ChiTietDonHang
                {
                    DonHangID = donHang.ID,
                    DoAnNhanhID = item.DoAnNhanhID,
                    SoLuong = item.SoLuong
                };
                _context.Set<ChiTietDonHang>().Add(chiTiet);
            }
            _context.SaveChanges();

            // Xóa giỏ hàng sau khi thanh toán thành công
            HttpContext.Session.Remove("GioHang");
            TempData["Success"] = "Thanh toán thành công! Đơn hàng của bạn đã được lưu.";
            return RedirectToAction(nameof(Index));
        }

        // Lấy giỏ hàng từ Session
        private List<ChiTietDonHang> GetGioHang()
        {
            var gioHangJson = HttpContext.Session.GetString("GioHang");
            return string.IsNullOrEmpty(gioHangJson)
                ? new List<ChiTietDonHang>()
                : JsonConvert.DeserializeObject<List<ChiTietDonHang>>(gioHangJson);
        }

        // Lưu giỏ hàng vào Session
        private void SaveGioHang(List<ChiTietDonHang> gioHang)
        {
            HttpContext.Session.SetString("GioHang", JsonConvert.SerializeObject(gioHang));
        }
    }
}