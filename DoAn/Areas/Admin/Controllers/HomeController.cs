using DoAn.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private Model1 db = new Model1();
        // GET: Admin/Home
        public ActionResult Index(string fill)
        {
            if (Session["hotenadmin"] != null)
            {
                DateTime ngayhomnay = DateTime.Now;
                List<DonHang> dsdh = db.DonHangs.Where(d => d.ngaydat.Day == DateTime.Now.Day).ToList();
                List<KhachHang> dskhh = db.KhachHangs.Where(o => o.ngaydangky.Day == DateTime.Now.Day).ToList();
                decimal doanhthu = 0;
                foreach(var item in dsdh)
                {
                    doanhthu += (decimal)item.tongtien;
                }
                ViewBag.khachhang = dskhh.Count;
                ViewBag.donhang = dsdh.Count;
                ViewBag.doanhthu = doanhthu;
                DateTime startDate = DateTime.Now;

                List<decimal> thongkedoanhthu = new List<decimal>();
                for (int month = 1; month <= 12; month++)
                {
                    var doanhthuthang = (db.DonHangs
                        .Where(s => s.ngaydat.Year == ngayhomnay.Year && s.ngaydat.Month == month)
                        .Sum(s => s.tongtien));
                    if(doanhthuthang == null)
                    {
                        thongkedoanhthu.Add(0);
                    }
                    else
                    {
                        thongkedoanhthu.Add((decimal)doanhthuthang);
                    }
                }

                ViewBag.doanhthuthang = thongkedoanhthu;
                
                if (!string.IsNullOrEmpty(fill))
                {
                    var orderCount = 0;
                    decimal doanhthun = 0;
                    var userCount = 0;
                    switch (fill)
                    {
                        case "1":
                            List<DonHang> ds = db.DonHangs.Where(o => o.ngaydat.Day == DateTime.Now.Day).ToList();
                            orderCount = ds.Count;
                            foreach (var item in ds)
                            {
                                doanhthun += (decimal)item.tongtien;
                            }
                            List<KhachHang> dskh = db.KhachHangs.Where(o => o.ngaydangky.Day == DateTime.Now.Day).ToList();
                            userCount = dskh.Count;
                            break;
                        case "2":
                            DateTime batDau = ngayhomnay.AddDays(-(int)ngayhomnay.DayOfWeek);
                            DateTime ketThuc = batDau.AddDays(6);
                            ds = db.DonHangs.Where(dh => dh.ngaydat >= batDau && dh.ngaydat <= ketThuc).ToList();
                            dskh = db.KhachHangs.Where(dh => dh.ngaydangky >= batDau && dh.ngaydangky <= ketThuc).ToList();
                            orderCount = ds.Count;
                            userCount = dskh.Count;
                            foreach (var item in ds)
                            {
                                doanhthun += (decimal)item.tongtien;
                            }
                            break;
                        case "3":
                            orderCount = db.DonHangs.Count(o => o.ngaydat.Month == DateTime.Now.Month);
                            userCount = db.KhachHangs.Count(o => o.ngaydangky.Month == DateTime.Now.Month);
                            ds = db.DonHangs.Where(o => o.ngaydat.Month == DateTime.Now.Month).ToList();
                            foreach (var item in ds)
                            {
                                doanhthun += (decimal)item.tongtien;
                            }
                            break;
                        case "4":
                            orderCount = db.DonHangs.Count();
                            userCount = db.KhachHangs.Count();
                            ds = db.DonHangs.ToList();
                            foreach (var item in ds)
                            {
                                doanhthun += (decimal)item.tongtien;
                            }
                            break;
                    }
                    
                    return Json(new { success = true, dh = orderCount, dt = doanhthun, kh = userCount });
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}