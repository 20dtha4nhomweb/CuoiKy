namespace CuoiKy.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("GioHang")]
    public partial class GioHang
    {
        DataMyPhamContext data = new DataMyPhamContext();

        public GioHang() { }
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaSP { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaTK { get; set; }

        public int? SoLuong { get; set; }

        public virtual SanPham SanPham { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }

        public Double? ThanhTien
        {
            get { return SanPham.Gia * SoLuong; }
        }

        public GioHang(int idSP)
        {
            MaSP = idSP;
            SanPham = data.SanPhams.Single(s => s.MaSP == MaSP);
            //MaTK = user;
            SoLuong = 1;
        }
        public GioHang(int idSP, int maTK)
        {
            this.MaSP = idSP;
            this.MaTK = maTK;
        }
    }
}
