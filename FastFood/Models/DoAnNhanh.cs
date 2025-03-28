using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace FastFood.Models  // Hoặc QL_doannhanh.Models nếu tên dự án khác
{
    public class DoAnNhanh
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên món ăn không được để trống")]
        public string MonAn { get; set; } = null!;

        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string MoTa { get; set; } = null!;

        [Required(ErrorMessage = "Giá không được để trống")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Gia { get; set; }

        // Lưu đường dẫn hình ảnh; nếu không có file upload, mặc định là /images/default.jpg
        public string HinhAnhUrl { get; set; } = "/images/default.jpg";

        // Thuộc tính dùng để nhận file upload từ form, không map vào DB.
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Chọn danh mục cho món ăn")]
        public int DanhMucID { get; set; }

        // Quan hệ với DanhMuc
        public DanhMuc? DanhMuc { get; set; }
    }
}
