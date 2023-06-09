﻿using System;
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
using System.Net.Mail;
using static System.Net.WebRequestMethods;
using System.Web.Helpers;
using System.Security.Principal;
using System.Data.Entity.Migrations;
using Microsoft.Ajax.Utilities;

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
                var email = taiKhoan.Email;
                var dienthoai = taiKhoan.SDT;
                if(taiKhoan.TenKhachHang.IsNullOrWhiteSpace()  || taiKhoan.Email.IsNullOrWhiteSpace() || taiKhoan.SDT.IsNullOrWhiteSpace() || 
                    taiKhoan.GioiTinh.IsNullOrWhiteSpace() || taiKhoan.NamSinh.ToString().IsNullOrWhiteSpace() || taiKhoan.DiaChi.IsNullOrWhiteSpace())
                {
                    ViewData["NotNull"] = "Không được trống";
                    return this.Edit(taiKhoan.MaTK);
                }
                var ngaysinh = String.Format("{0:MM/dd/yyyy}", taiKhoan.NamSinh);
                var now = String.Format("{0:MM/dd/yyyy}", DateTime.Now);
                Regex regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match matchMail = regexMail.Match(email);
                Regex regexPhone = new Regex(@"^(84|0[3|5|7|8|9])+([0-9]{8})\b");
                Match matchPhone = regexPhone.Match(dienthoai);
                if (!matchMail.Success)
                {
                    ViewData["EmailWrong"] = "Email phải đúng định dạng";
                    return this.Edit(taiKhoan.MaTK);
                }
                if (/*int.Parse(dienthoai) != 0 &&*/ !matchPhone.Success)
                {
                    ViewData["NumWrong"] = "Số điện thoại phải đúng định dạng";
                    return this.Edit(taiKhoan.MaTK);
                }
                if (DateTime.Parse(ngaysinh) > DateTime.Parse(now))
                {
                    ViewData["BirthWrong"] = "Ngày sinh phải bé hơn ngày hiện tại";
                    return this.Edit(taiKhoan.MaTK);
                }
                data.Entry(taiKhoan).State = EntityState.Modified;
                data.SaveChanges();
            }
            return RedirectToAction("Details","TaiKhoans",new {@id = taiKhoan.MaTK});
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
                    tk.MatKhau = BCrypt.Net.BCrypt.HashPassword(matkhau);
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
                bool verified = BCrypt.Net.BCrypt.Verify( matkhau, kh.MatKhau);
                if (verified == true)
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
            if (kh.PhanQuyen.Contains("admin"))
                return RedirectToAction("IndexAdmin","Home");
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
                tk.MatKhau = BCrypt.Net.BCrypt.HashPassword(matkhau);
                tk.TenKhachHang = hoten;
                tk.Email = email;
                tk.SDT = dienthoai;
                tk.PhanQuyen = "admin";
                tk.NamSinh = DateTime.Parse(ngaysinh);
                tk.DiaChi = diachi;
                data.TaiKhoans.Add(tk);
                data.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            
        }

        //Page quản lý admin & account khách
        public ActionResult PageAdmin()
        {
            var listAccount = data.TaiKhoans.OrderBy(s => s.PhanQuyen).ToList();
            return View(listAccount);
        }
        public JsonResult SendOTP(TaiKhoan tk)
        {
            bool Valid = false;
            TaiKhoan checkEmail = data.TaiKhoans.SingleOrDefault(x => x.Email == tk.Email && x.TenDangNhap == tk.TenDangNhap);
            if (checkEmail != null)
            {
                Valid = true;
                Random rand = new Random();
                int OTP = rand.Next(100000, 1000000);
                Session["OTP"] = OTP.ToString();
                var fromAddress = new MailAddress("nguyentrongquy612@gmail.com");
                var toAddress = new MailAddress(tk.Email);
                const string fromPass = "lhfxhqggiwpxhqat";
                const string subject = "OTP code";
                string body = "Đây là mã xác thực của bạn:\nVui lòng không chia sẻ mã này cho bất kì ai khác:\n" + "\t" + OTP.ToString();
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPass),
                    Timeout = 200000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                })
                {
                    smtp.Send(message);
                }
            }

            return Json(Valid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RecoveryPass(TaiKhoan tk, FormCollection collection)
        {
            var otp = collection["OTP"];
            var user = collection["User"];
            string kq = "NotValid";
            var checkMail = data.TaiKhoans.SingleOrDefault(x => x.Email == tk.Email);
            if (Session["OTP"] == null)
            {
                kq = "WrongOTP";
            }
            else if (checkMail != null && otp == Session["OTP"].ToString())
            {
                kq = "Valid";
                checkMail.MatKhau = BCrypt.Net.BCrypt.HashPassword(tk.MatKhau);
                data.TaiKhoans.AddOrUpdate(checkMail);
                data.SaveChanges();
                Session.Clear();
                Session.Abandon();
            }
            else if (otp != Session["OTP"].ToString())
            {
                kq = "WrongOTP";
            }
            return Json(kq, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RecoveyPassLayout()
        {
            return View();
        }
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePass(FormCollection collection, int maTK)
        {
            var oldPass = collection["OldPass"];
            var pass = collection["Password"];
            var confirmPass = collection["ConfirmPass"];
            TaiKhoan tk = data.TaiKhoans.FirstOrDefault(x => x.MaTK == maTK);
            Regex regexPass = new Regex(@"(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{8,})");
            Match matchPass = regexPass.Match(pass);
            bool verified = BCrypt.Net.BCrypt.Verify(oldPass, tk.MatKhau);
            if(verified == true)
            {
                if (pass != confirmPass)
                {
                    ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau";
                    return this.ChangePass();
                }
                if (!matchPass.Success)
                {
                    ViewData["WeakPass"] = "Mật khẩu phải có ít nhất 1 ký tự hoa, thường, dặc biệt, số và độ dài ngắn nhất bằng 8";
                    return this.ChangePass();
                }
                tk.MatKhau = BCrypt.Net.BCrypt.HashPassword(pass);
                data.TaiKhoans.AddOrUpdate(tk);
                data.SaveChanges();
                Session["TaiKhoan"] = null;
                Session["User"] = null;
                Session.Clear();
                Logout();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["WrongOldPass"] = "Mật khẩu cũ bạn nhập hông có đúng!";
                return this.ChangePass();
            }
        }
    }
}
