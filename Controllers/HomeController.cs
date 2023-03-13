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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Resigter(TaiKhoan tk)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var check = context.TaiKhoans.FirstOrDefault(a => a.TenDangNhap == a.TenDangNhap);
        //        if (check != null)
        //        {

        //            context.Configuration.ValidateOnSaveEnabled = false;
        //            context.TaiKhoans.Add(tk);
        //            context.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ViewBag.error = "Tên tài khoản đã tồn tại";
        //            return View();
        //        }
        //    }
        //    return View();
        //}

        //Login chưa được
        //public ActionResult Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(string tenTK, string matKhau)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var data = context.TaiKhoans.Where(s=>s.TenDangNhap.Equals(tenTK)&&s.MatKhau.Equals(matKhau));
        //        if(data.Count() > 0)
        //        {
        //            Session["idUser"] = data.FirstOrDefault().TenDangNhap;
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ViewBag.error = "Tài khoản hoặc mật khẩu không chính xác!";
        //            return RedirectToAction("Login");
        //        }
        //    }
        //    return View();
        //}
        




    }
}