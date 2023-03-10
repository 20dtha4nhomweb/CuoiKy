using CuoiKy.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuoiKy.Controllers
{
    public class HomeController : Controller
    {
        DataMyPhamContext context = new DataMyPhamContext();
        public ActionResult Index(int?page)
        {
            if (page == null)
            {
                page = 1;
            }
            var all_SanPham = (from ss in context.SanPhams select ss).OrderBy(m => m.TenSanPham);
            int pageSize = 20;
            int pageNum = page ?? 1;

            return View(all_SanPham.ToPagedList(pageNum, pageSize));            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Resigter()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Resigter(TaiKhoan tk)
        {
            if(ModelState.IsValid)
            {
                var check = context.TaiKhoans.FirstOrDefault(a => a.TenDangNhap == a.TenDangNhap);
                if (check != null)
                {

                    context.Configuration.ValidateOnSaveEnabled = false;
                    context.TaiKhoans.Add(tk);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Tên tài khoản đã tồn tại";
                    return View();
                }                   
            }
            return View();
        }

        public ActionResult NhapThongTin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NhapThongTin(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                    context.Configuration.ValidateOnSaveEnabled = false;
                    context.KhachHangs.Add(kh);
                    context.SaveChanges();
                    return RedirectToAction("Resigter");                    
            }
            return View();
        }
    }
}