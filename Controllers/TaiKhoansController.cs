using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CuoiKy.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using static CuoiKy.Models.TaiKhoan;
using System.Text.RegularExpressions;
using System.ComponentModel.Design;

namespace CuoiKy.Controllers
{
    public class TaiKhoansController : Controller
    {
        private DataMyPhamContext data = new DataMyPhamContext();

        // GET: TaiKhoans
        public ActionResult Index()
        {
            return View(data.TaiKhoans.ToList());
        }

        // GET: TaiKhoans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = data.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }
        // GET: TaiKhoans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = data.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: TaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTK,TenDangNhap,MatKhau,TenKhachHang,Email,SDT,GioiTinh,NamSinh,DiaChi,PhanQuyen")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                data.Entry(taiKhoan).State = EntityState.Modified;
                data.SaveChanges();
                return RedirectToAction("PageAdmin");
            }
            return View(taiKhoan);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = data.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaiKhoan taiKhoan = data.TaiKhoans.Find(id);
            data.TaiKhoans.Remove(taiKhoan);
            data.SaveChanges();
            return RedirectToAction("PageAdmin");
        }

        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, TaiKhoan tk)
        {

            var tendangnhap = collection["TenDangNhap"];
            var matkhau = collection["MatKhau"];
            var MatKhauXacNhan = collection["MatKhauXacNhan"];
            var hoten = collection["TenKhachHang"];
            var email = collection["Email"];
            var dienthoai = collection["SDT"];
            var diachi = collection["DiaChi"];
            //string temp = dienthoai;
            //char check = temp[0];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["NamSinh"]);
            Regex regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match matchMail = regexMail.Match(email);
            Regex regexPhone = new Regex(@"^(84|0[3|5|7|8|9])+([0-9]{8})\b");
            Match matchPhone = regexPhone.Match(dienthoai);
            Regex regexPass = new Regex(@"(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{8,})");
            Match matchPass = regexPass.Match(matkhau);
            var checkUser = data.TaiKhoans.FirstOrDefault(x => x.TenDangNhap == tendangnhap);
            if (checkUser != null)
            {
                ViewData["UserExist"] = "Tên đăng nhập đã tồn tại";
                return this.DangKy();
            }
            if (!matchPass.Success)
            {
                ViewData["WeakPass"] = "Mật khẩu phải có ít nhất 1 ký tự hoa, thường, dặc biệt, số và độ dài ngắn nhất bằng 8";
                return this.DangKy();
            }
            if (String.IsNullOrEmpty(MatKhauXacNhan))
            {
                ViewData["NhapXNMK"] = "Phải nhập mật khẩu xác nhận!";
            }
            if (matkhau != MatKhauXacNhan)
            {
                ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau";
                return this.DangKy();
            }
            if (!matchMail.Success)
            {
                ViewData["EmailWrong"] = "Email phải đúng định dạng";
                return this.DangKy();
            }
            if (/*int.Parse(dienthoai) != 0 &&*/ !matchPhone.Success)
            {
                ViewData["NumWrong"] = "Số điện thoại phải đúng định dạng";
                return this.DangKy();
            }
            
            if (DateTime.Parse(ngaysinh) > DateTime.Now)
            {
                ViewData["BirthWrong"] = "Ngày sinh phải bé hơn ngày hiện tại";
                return this.DangKy();
            }            
            else
            {             
                    tk.TenDangNhap = tendangnhap;
                    tk.MatKhau = matkhau;
                    tk.TenKhachHang = hoten;
                    tk.Email = email;
                    tk.SDT = dienthoai;
                    tk.PhanQuyen = "user";
                    tk.NamSinh = DateTime.Parse(ngaysinh);
                    tk.DiaChi = diachi;
                    data.TaiKhoans.Add(tk);
                    data.SaveChanges();
                    return RedirectToAction("Index","Home");                           
            }            
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendangnhap = collection["TenDangNhap"];
            var matkhau = collection["MatKhau"];
            TaiKhoan kh = data.TaiKhoans.FirstOrDefault(x => x.TenDangNhap == tendangnhap);            
            if (kh == null)
            {
                ViewData["ErrorAccount"] = "Tên đăng nhập không tồn tại";
                return this.DangNhap();
            }
            if (kh != null)
            {
                if(kh.MatKhau == matkhau)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = kh.MaTK;
                    Session["User"] = kh.TenDangNhap;
                    Session["Account"] = kh.PhanQuyen;
                    Session["FullTaiKhoan"] = kh;
                }    
                else
                {
                    ViewData["ErrorPass"] = "Mật khẩu không đúng";
                    return this.DangNhap();
                }
            }                     
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Logout()
        {
            Session["TaiKhoan"] = null;
            Session["User"] = null;
            return RedirectToAction("Index", "Home");
        }

        //Đăng ký tài khoản admin
        public ActionResult DangKyAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKyAdmin(FormCollection collection, TaiKhoan tk)
        {
            var tendangnhap = collection["TenDangNhap"];
            var matkhau = collection["MatKhau"];
            var MatKhauXacNhan = collection["MatKhauXacNhan"];
            var hoten = collection["TenKhachHang"];
            var email = collection["Email"];
            var dienthoai = collection["SDT"];
            var diachi = collection["DiaChi"];
            //string temp = dienthoai;
            //char check = temp[0];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["NamSinh"]);
            Regex regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match matchMail = regexMail.Match(email);
            Regex regexPhone = new Regex(@"^(84|0[3|5|7|8|9])+([0-9]{8})\b");
            Match matchPhone = regexPhone.Match(dienthoai);
            Regex regexPass = new Regex(@"(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{8,})");
            Match matchPass = regexPass.Match(matkhau);
            var checkUser = data.TaiKhoans.FirstOrDefault(x => x.TenDangNhap == tendangnhap);
            if (checkUser != null)
            {
                ViewData["UserExist"] = "Tên đăng nhập đã tồn tại";
                return this.DangKyAdmin();
            }
            if (!matchPass.Success)
            {
                ViewData["WeakPass"] = "Mật khẩu phải có ít nhất 1 ký tự hoa, thường, dặc biệt, số và độ dài ngắn nhất bằng 8";
                return this.DangKy();
            }
            if (String.IsNullOrEmpty(MatKhauXacNhan))
            {
                ViewData["NhapXNMK"] = "Phải nhập mật khẩu xác nhận!";
                return this.DangKy();
            }
            if (matkhau != MatKhauXacNhan)
            {
                ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau";
                return this.DangKy();
            }
            if (!matchMail.Success)
            {
                ViewData["EmailWrong"] = "Email phải đúng định dạng";
                return this.DangKyAdmin();
            }
            if (int.Parse(dienthoai) != 0 && !matchPhone.Success)
            {
                ViewData["NumWrong"] = "Num phải đúng định dạng";
                return this.DangKyAdmin();
            }
            if (!matchMail.Success)
            {
                ViewData["EmailWrong"] = "Email phải đúng định dạng";
                return this.DangKyAdmin();
            }
            if (DateTime.Parse(ngaysinh) > DateTime.Now)
            {
                ViewData["BirthWrong"] = "Ngày sinh phải bé hơn ngày hiện tại";
                return this.DangKyAdmin();
            }            
            else
            {    
                
                    tk.TenDangNhap = tendangnhap;
                    tk.MatKhau = matkhau;
                    tk.TenKhachHang = hoten;
                    tk.Email = email;
                    tk.SDT = dienthoai;
                    tk.PhanQuyen = "admin";
                    tk.NamSinh = DateTime.Parse(ngaysinh);
                    tk.DiaChi = diachi;
                    data.TaiKhoans.Add(tk);
                    data.SaveChanges();
                    return RedirectToAction("Index","Home");                
            }
            
        }

        //Page quản lý admin & account khách
        public ActionResult PageAdmin()
        {
            var listAccount = data.TaiKhoans.OrderBy(s => s.PhanQuyen).ToList();
            return View(listAccount);
        }

        
    }
}
