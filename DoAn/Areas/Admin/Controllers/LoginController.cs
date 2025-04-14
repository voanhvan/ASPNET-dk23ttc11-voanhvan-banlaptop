using DoAn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        Model1 db = new Model1();
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
         
        public ActionResult DangNhap(string email, string password)
        {
            var khachHang = db.KhachHangs.Where(s => s.email == email && s.matkhau == password && s.chucvu == true).FirstOrDefault();
            if(khachHang != null )
            {
                Session["hotenadmin"] = khachHang.hoten;
                Session.Timeout = 100;
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public ActionResult DangXuat()
        {
            Session["hoten"] = null;
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}