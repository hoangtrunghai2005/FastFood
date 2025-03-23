using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FastFood.Models;
using Newtonsoft.Json;

namespace FastFood.Models
{
    public class DonHang
    {
        public int ID { get; set; }

        [Required]
        public DateTime NgayDat { get; set; }

        [Required]
        [StringLength(100)]
        public string TenKhachHang { get; set; }

        [Required]
        [StringLength(200)]
        public string DiaChi { get; set; }

        [Required]
        [Phone]
        public string SoDienThoai { get; set; }

        [Required]
        public string PhuongThucThanhToan { get; set; }

        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }

    public class ChiTietDonHang
    {
        public int ID { get; set; }

        [Required]
        public int DonHangID { get; set; }

        [Required]
        public int DoAnNhanhID { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public DoAnNhanh DoAnNhanh { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public DonHang DonHang { get; set; }
    }
}
