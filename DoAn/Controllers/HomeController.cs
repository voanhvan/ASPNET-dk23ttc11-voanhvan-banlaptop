using DoAn.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Controllers
{
    public class HomeController : Controller
    {
        private Model1 db = new Model1();
        public ActionResult Index()
        {
            List<SanPham> ds = new List<SanPham>();
            List<SanPham> ds_sale = db.SanPhams.Where(s => s.trangthai == true).Take(10).ToList();
            List<SanPham> ds_new = db.SanPhams.Where(s => s.trangthai == true).OrderByDescending(s => s.masp).Take(15).ToList();
            List<HangSP> ds_hang = db.HangSPs.ToList();
            ViewBag.sale = ds_sale;
            ViewBag.dsnew = ds_new;
            Session["dshang"] = ds_hang;
            Session.Timeout = 100;
            ViewBag.dsh = ds_hang;
            return View();
        }

        public ActionResult DetailProduct()
        {
            return View();
        }

        //public ActionResult Cart()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}

        //public ActionResult CartDetail()
        //{
        //    return View();
        //}

        //public ActionResult Data()
        //{
        //    List<GioHang> listGH = db.GioHangs.ToList();
        //    ViewBag.ds = listGH;
        //    return View("_Layout");
        //}

        public ActionResult LienHe()
        {
            return View();
        }

        public ActionResult TinTuc()
        {
            return View();
        }
    }
}