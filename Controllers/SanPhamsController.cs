using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DoAn.Models;
using PagedList;

namespace DoAn.Controllers
{
    public class SanPhamsController : Controller
    {
        private Model1 db = new Model1();

        public ActionResult Index(int? page, List<int> brands, string price)
        {
            var searchString = Request.Form["timkiem"];
            int pageSize = 16;
            int pageNumber = page ?? 1;
            List<SanPham> sanPhams = db.SanPhams.Include(s => s.HangSP).Where(s=>s.trangthai==true).ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                sanPhams = db.SanPhams.Where(sp => sp.tensp.Contains(searchString)).ToList();
            }
            if (brands != null && brands.Count > 0)
            {
                sanPhams = sanPhams.Where(sp => brands.Contains(sp.HangSP.mahang)).ToList();
                
            }
            if(!string.IsNullOrEmpty(price))
            {
                switch (price)
                {
                    case "price-1":
                        sanPhams = sanPhams.OrderBy(s => s.giaban).ToList();
                        break;
                    case "price-2":
                        sanPhams = sanPhams.OrderByDescending(s => s.giaban).ToList();
                        break;
                    case "price-3":
                        sanPhams = sanPhams.Where(s => s.giaban <= 10000000).ToList();
                        break;
                    case "price-4":
                        sanPhams = sanPhams.Where(s => s.giaban >= 10000000 && s.giaban <= 15000000).ToList();
                        break;
                    case "price-5":
                        sanPhams = sanPhams.Where(s => s.giaban > 15000000 && s.giaban <= 20000000).ToList();
                        break;
                    case "price-6":
                        sanPhams = sanPhams.Where(s => s.giaban > 20000000 && s.giaban <= 25000000).ToList();
                        break;
                    case "price-7":
                        sanPhams = sanPhams.Where(s => s.giaban > 25000000).ToList();
                        break;
                    
                }
            }
            List<HangSP> hsp = db.HangSPs.Take(7).ToList();
            ViewBag.dsh = hsp;
            ViewBag.searchString = searchString;
            return View(sanPhams.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            List<SanPham> listsp = db.SanPhams.Where(s=>s.mahang == sanPham.mahang).ToList();           
            listsp.Remove(sanPham);
            ViewBag.ds = listsp;
            return View(sanPham);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult AddCart(int id)
        {
            if (Session["ma"] == null)
            {
                string returnUrl = Url.Action("Index", "SanPhams");
                Session["ReturnUrl"] = returnUrl;
                
                return Json(new { success = false});
            }
            else
            {
                GioHang gh = new GioHang();
                gh.makhachhang = (int)Session["ma"];
                gh.masp = id;
                gh.soluong = 1;
                db.GioHangs.Add(gh);
                db.SaveChanges();
                Session["giohang"] = (int)Session["giohang"] + 1;
                return Json(new { success = true }) ;
            }
        }
        public ActionResult AddCart1(int id)
        {
            if (Session["ma"] == null)
            {
                string returnUrl = Url.Action("Index", "Home");
                Session["ReturnUrl"] = returnUrl;

                return Json(new { success = false });
            }
            else
            {
                GioHang gh = new GioHang();
                gh.makhachhang = (int)Session["ma"];
                gh.masp = id;
                gh.soluong = 1;
                db.GioHangs.Add(gh);
                db.SaveChanges();
                Session["giohang"] = (int)Session["giohang"] + 1;
                return Json(new { success = true });
            }
        }
        public ActionResult AddCart2(int id, int soluong)
        {
            if (Session["ma"] == null)
            {
                RouteValueDictionary routeValues = new RouteValueDictionary();
                routeValues["id"] = id;
                string returnUrl = Url.Action("Details", "SanPhams", routeValues);
                Session["ReturnUrl"] = returnUrl;
                return Json(new { success = false });
            }
            else
            {
                SanPham sp = db.SanPhams.Find(id);
                if (soluong > sp.soluong)
                {
                    return Json(new { success = true, message = "F" });
                }
                else
                {
                    GioHang gh = new GioHang();
                    gh.makhachhang = (int)Session["ma"];
                    gh.masp = id;
                    gh.soluong = soluong;
                    db.GioHangs.Add(gh);
                    db.SaveChanges();
                    Session["giohang"] = (int)Session["giohang"] + 1;
                    return Json(new { success = true, message = "T" });
                }
            }
        }
    }
}
