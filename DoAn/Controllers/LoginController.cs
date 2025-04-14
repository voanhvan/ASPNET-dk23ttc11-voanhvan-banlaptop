using DoAn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DoAn.Controllers
{
    public class LoginController : Controller
    {
        Model1 db = new Model1();
        // GET: Login
        public ActionResult Index()
        {
            return View(); 
        }

        [HttpPost]
        public ActionResult DangNhap()
        {
            string mail = Request.Form["email"];
            string password = Request.Form["password"];
            var user = db.KhachHangs.Where(s => s.email == mail && s.matkhau == password).FirstOrDefault();

            if (user == null)
            {
                Console.WriteLine(password);
                return RedirectToAction("Index", "Login");

            }
            else
            {
                if(user.chucvu == true)
                {
                    return RedirectToAction("Index", "Admin/Login");
                }
                Session["ma"] = user.makhachhang;
                Session["hoten"] = user.hoten;
                Session["giohang"] = db.GioHangs.Where(s=>s.makhachhang == user.makhachhang).ToList().Count;
                string returnUrl = Session["ReturnUrl"] as string;
                Session.Remove("ReturnUrl");
                Session.Timeout = 100;
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    // Chuyển hướng đến returnUrl nếu tồn tại
                    return Redirect(returnUrl);  
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
                
            }
        }
        

        [HttpPost]     
        public ActionResult DangKy(string hoten, string email, string pass, string repass)
        {
            //string hoten = Request.Form["rhoten"];
            //string mail = Request.Form["remail"];
            //string password = Request.Form["rpassword"];
            //string re_password = Request.Form["rrpassword"];
            if(pass == repass)
            {
                KhachHang kh = new KhachHang();
                kh.hoten = hoten;
                kh.email = email;
                kh.matkhau = pass;
                kh.ngaydangky = DateTime.Now;
                db.KhachHangs.Add(kh);
                db.SaveChanges();
                return Json(new {success = true});
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public ActionResult SignOut()
        {
            Session["ma"] = null;
            Session["hoten"] = null;
            Session["giohang"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Register(string hoten, string email, string pass, string repass)
        {
            if (pass == repass)
            {
                KhachHang kh = new KhachHang();
                kh.hoten = hoten;
                kh.email = email;
                kh.matkhau = pass;
                db.KhachHangs.Add(kh);
                db.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}