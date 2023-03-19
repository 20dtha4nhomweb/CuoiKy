using CuoiKy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.EnterpriseServices.CompensatingResourceManager;
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
            int maTK = (int)Session["TaiKhoan"];
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            var loadGH = data.GioHangs.Where(gh => gh.MaTK == maTK).ToList();
            lstGiohang = loadGH;
            if (lstGiohang == null)
            {
                lstGiohang = new List<GioHang>();
                Session["GioHang"] = lstGiohang;
            }
            //else if (lstGiohang == null)
            //{
            //    lstGiohang = new List<GioHang>();
            //    Session["GioHang"] = lstGiohang;
            //}
            else 
            {
                foreach (GioHang gh in loadGH)
                {
                    gh.SanPham = (SanPham)data.SanPhams.FirstOrDefault(x => x.MaSP == gh.MaSP);
                    gh.TaiKhoan = (TaiKhoan)data.TaiKhoans.FirstOrDefault(x => x.MaTK == gh.MaTK);
                    if (lstGiohang == null)
                    {
                        lstGiohang = new List<GioHang>();
                        lstGiohang.Add(gh);
                    }
                    else
                    {
                        if (!lstGiohang.Contains(gh))
                            lstGiohang.Add(gh);
                        else Session["GioHang"] = lstGiohang;
                    }
                }
            }
            //Session["GioHang"] = lstGiohang;
            return lstGiohang;
        }

        public ActionResult ThemGioHang(int id, string strURL)
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
            int maTK = (int)Session["TaiKhoan"];
            List<GioHang> lstGiohang = Laygiohang();
            //lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.Find(s => s.MaSP == id);
            GioHang temp = new GioHang(id, maTK);
            if (sanpham == null)
            {
                sanpham = new GioHang(id);
                sanpham.MaTK = maTK;
                lstGiohang.Add(sanpham);               
                temp.SoLuong = sanpham.SoLuong;
                data.GioHangs.Add(temp);
                data.SaveChanges();
                return Redirect(strURL);
            }
            else 
            {
                sanpham.SoLuong += 1;
                lstGiohang.Add(sanpham);
                temp.SoLuong = sanpham.SoLuong; ;
                data.GioHangs.AddOrUpdate(temp);
                data.SaveChanges();
                return Redirect(strURL);
            }
        }

        private int? TongSoLuong()
        {
            int? tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            lstGiohang = Laygiohang();
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
            lstGiohang = Laygiohang();
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
            lstGiohang = Laygiohang();
            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.ThanhTien);
            }
            return tt;
        }


        // GET: GioHang
        public ActionResult GioHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
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
            foreach (var item in lstGiohang)
            {
                var temp = data.GioHangs.FirstOrDefault(x => x.MaTK == item.MaTK && x.MaSP == item.MaSP);
                data.GioHangs.Remove(temp);
                data.SaveChanges();
            }
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
            Session["GioHang"] = null;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            lstGiohang = Laygiohang();
            Session["GioHang"] = lstGiohang;
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        public ActionResult Dathang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            TaiKhoan kh = (TaiKhoan)Session["FullTaiKhoan"];
            SanPham s = new SanPham();
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            //lstGiohang = Laygiohang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            dh.MaTK = kh.MaTK;
            dh.NgayLap = DateTime.Now;
            data.DonHangs.Add(dh);
            data.SaveChanges();
            foreach (var item in lstGiohang)
            {
                var temp = data.GioHangs.FirstOrDefault(x => x.MaTK == item.MaTK && x.MaSP == item.MaSP);
                data.GioHangs.Remove(temp);
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