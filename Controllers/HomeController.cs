using CuoiKy.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuoiKy.Controllers
{
    public class HomeController : Controller
    {
        DataMyPhamContext context = new DataMyPhamContext();
        public ActionResult Index(int? page, string SearchString)
        {
            int pageSize = 8;
            int pageNum = page ?? 1;
            var all_SanPham = context.SanPhams.OrderBy(s => s.TenSanPham);
            var all_SanPhamTK = context.SanPhams.OrderBy(m => m.TenSanPham).Where(sp => sp.TenSanPham.ToUpper().Contains(SearchString.ToUpper()));
            page = 1;
            if (SearchString == null || SearchString == "")
                return View(all_SanPham.ToPagedList(pageNum, pageSize));
            else if(all_SanPhamTK != null)
                return View(all_SanPhamTK.ToPagedList(pageNum, pageSize));
            else
                return View(all_SanPham.ToPagedList(pageNum, pageSize));
        }
        public ActionResult IndexAdmin(int? page, string SearchString)
        {
            int pageSize = 8;
            int pageNum = page ?? 1;
            var all_SanPham = context.SanPhams.OrderBy(s => s.TenSanPham);
            var all_SanPhamTK = context.SanPhams.OrderBy(m => m.TenSanPham).Where(sp => sp.TenSanPham.ToUpper().Contains(SearchString.ToUpper()));
            page = 1;
            if (SearchString == null || SearchString == "")
                return View(all_SanPham.ToPagedList(pageNum, pageSize));
            else if (all_SanPhamTK != null)
                return View(all_SanPhamTK.ToPagedList(pageNum, pageSize));
            else
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
    }
}