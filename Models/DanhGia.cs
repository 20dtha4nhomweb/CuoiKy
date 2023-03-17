namespace CuoiKy.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhGia")]
    public partial class DanhGia
    {
        [Key]
        public int MaDG { get; set; }

        public int MaTK { get; set; }

        public int MaSP { get; set; }

        [StringLength(500)]
        public string NoiDung { get; set; }

        [StringLength(100)]
        public string HinhAnh { get; set; }

        public int? Sao { get; set; }

        public DateTime? Ngay { get; set; }

        public virtual SanPham SanPham { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
