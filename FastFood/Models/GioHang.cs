using FastFood.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Models
{
    public class GioHang
    {
        [Key]
        public int GioHangID { get; set; }

        [Required]
        public string UserID { get; set; } // ID người dùng để liên kết giỏ hàng với người dùng

        public virtual ICollection<GioHangChiTiet> GioHangChiTiets { get; set; }
    }

    public class GioHangChiTiet
    {
        [Key]
        public int GioHangChiTietID { get; set; }

        [Required]
        public int GioHangID { get; set; }

        [Required]
        public int DoAnNhanhID { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        [ForeignKey("GioHangID")]
        public virtual GioHang GioHang { get; set; }

        [ForeignKey("DoAnNhanhID")]
        public virtual DoAnNhanh DoAnNhanh { get; set; }
    }
}
