using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FastFood.Models;
using FastFood.Areas.Identity.Data;

namespace FastFood.Data
{
    public class ApplicationDbContext : IdentityDbContext<FastFoodUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Các DbSet khác
        public DbSet<DoAnNhanh> DoAnNhanhs { get; set; } = default!;
        public DbSet<DanhMuc> DanhMucs { get; set; } = default!;
        public DbSet<DonHang> DonHangs { get; set; } = default!;
        public DbSet<GioHang> GioHangs { get; set; } = default!;
        public DbSet<GioHangChiTiet> GioHangChiTiets { get; set; } = default!;
        public object ChiTietDonHangs { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed dữ liệu cho DanhMucs
            modelBuilder.Entity<DanhMuc>().HasData(
                new DanhMuc { DanhMucID = 1, TenDanhMuc = "Đồ Uống" },
                new DanhMuc { DanhMucID = 2, TenDanhMuc = "Món Chính" },
                new DanhMuc { DanhMucID = 3, TenDanhMuc = "Tráng Miệng" }
            );

            // Seed dữ liệu mẫu cho DoAnNhanhs
            modelBuilder.Entity<DoAnNhanh>().HasData(
                new DoAnNhanh
                {
                    ID = 1,
                    MonAn = "Burger bò",
                    MoTa = "Burger bò thơm ngon với phô mai tan chảy.",
                    Gia = 66000m,
                    HinhAnhUrl = "/images/buger_bo.jpg",
                    DanhMucID = 2
                },
                new DoAnNhanh
                {
                    ID = 2,
                    MonAn = "Khoai tây chiên",
                    MoTa = "Khoai tây chiên giòn rụm, vàng ươm.",
                    Gia = 30000m,
                    HinhAnhUrl = "/images/khoai_tay_chien.jpg",
                    DanhMucID = 2
                },
                new DoAnNhanh
                {
                    ID = 3,
                    MonAn = "Gà rán",
                    MoTa = "Gà rán giòn rụm và ngọt thịt bên trong. Gồm 2 đùi gà.",
                    Gia = 30000m,
                    HinhAnhUrl = "/images/ga_ran.jpg",
                    DanhMucID = 2
                },
                new DoAnNhanh
                {
                    ID = 4,
                    MonAn = "Pizza hải sản",
                    MoTa = "Pizza hải sản tươi ngon với nhiều loại hải sản.",
                    Gia = 175000m,
                    HinhAnhUrl = "/images/pizza_hs.jpg",
                    DanhMucID = 2
                },
                new DoAnNhanh
                {
                    ID = 5,
                    MonAn = "Coca-Cola®",
                    MoTa = "Thức uống có ga.",
                    Gia = 15000m,
                    HinhAnhUrl = "/images/coca.jpg",
                    DanhMucID = 1
                },
                new DoAnNhanh
                {
                    ID = 6,
                    MonAn = "Nước suối",
                    MoTa = "Nước suối tinh khiết.",
                    Gia = 20000m,
                    HinhAnhUrl = "/images/dasani.jpg",
                    DanhMucID = 1
                },
                new DoAnNhanh
                {
                    ID = 7,
                    MonAn = "Hot dog",
                    MoTa = "Bánh mì kẹp xúc xích và mù tạt.",
                    Gia = 20000m,
                    HinhAnhUrl = "/images/hotdog.jpg",
                    DanhMucID = 2
                }
            );
        }
    }
}
