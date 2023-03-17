using CuoiKy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuoiKy.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        DataMyPhamContext data = new DataMyPhamContext();
        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<GioHang>();
                Session["GioHang"] = lstGiohang;
            }
            return lstGiohang;
        }

        public ActionResult ThemGioHang(int id, string strURL)
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.Find(s => s.MaSP == id);
            sanpham.MaTK = (int)Session["TaiKhoan"];           
            var temp = data.GioHangs.FirstOrDefault(gh => gh.MaTK == sanpham.MaTK && gh.MaSP == sanpham.MaSP);
            if (sanpham == null || temp == null)
            {
                sanpham = new GioHang(id);
                lstGiohang.Add(sanpham);
                data.GioHangs.Add(sanpham);
                data.SaveChanges();
                return Redirect(strURL);
            }
            else
            {
                sanpham.SoLuong++;
                return Redirect(strURL);
            }
        }

        private int? TongSoLuong()
        {
            int? tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Sum(s => s.SoLuong);
            }
            return tsl;
        }

        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }

        private double? TongTien()
        {
            double? tt = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.ThanhTien);
            }
            return tt;
        }


        // GET: GioHang
        public ActionResult GioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }

        public ActionResult XoaGiohang(int id)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.MaSP == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.MaSP == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(s => s.MaSP == id);
            if (sanpham != null)
            {
                sanpham.SoLuong = int.Parse(collection["txtSoLg"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }

        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        public ActionResult Dathang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            TaiKhoan kh = (TaiKhoan)Session["TaiKhoan"];
            SanPham s = new SanPham();
            List<GioHang> lstGiohang = Laygiohang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            dh.MaTK = kh.MaTK;
            dh.NgayLap = DateTime.Now;

            data.DonHangs.Add(dh);
            data.SaveChanges();
            foreach (var item in lstGiohang)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaSP = item.MaSP;
                ctdh.MaDH = dh.MaDH;
                ctdh.SoLuong = item.SoLuong;
                ctdh.TinhTrang = "False";
                ctdh.NgayGiao = DateTime.Parse(ngaygiao);
                data.ChiTietDonHangs.Add(ctdh);
                data.SaveChanges();
            }
            data.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("Xacnhandonhang", "GioHang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}