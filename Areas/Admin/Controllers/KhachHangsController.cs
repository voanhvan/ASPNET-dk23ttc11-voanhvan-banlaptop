using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;

namespace DoAn.Areas.Admin.Controllers
{
    public class KhachHangsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Admin/KhachHangs
        public ActionResult Index()
        {
            return View(db.KhachHangs.ToList());
        }

        // GET: Admin/KhachHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/KhachHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "makhachhang,hoten,diachi,dienthoai,email,matkhau,trangthai,chucvu,ngaydangky")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> trangThaiList = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Khóa" },
                new SelectListItem { Value = "false", Text = "Hoạt động" }
            };
            List<SelectListItem> chucvuList = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Nhân viên" },
                new SelectListItem { Value = "false", Text = "Khách hàng" }
            };
            ViewBag.TrangThaiList = trangThaiList;
            ViewBag.chucvuList = chucvuList;
            return View(khachHang);
        }

        // POST: Admin/KhachHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "makhachhang,hoten,diachi,dienthoai,email,matkhau,trangthai,chucvu,tinh,huyen,xa,thon")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    KhachHang khachHang = db.KhachHangs.Find(id);
        //    if (khachHang == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(khachHang);
        //}

        // POST: Admin/KhachHangs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);
            db.DonHangChiTiets.RemoveRange(khachHang.DonHangs.SelectMany(dh => dh.DonHangChiTiets));
            db.DonHangs.RemoveRange(khachHang.DonHangs);
            db.GioHangs.RemoveRange(khachHang.GioHangs);
            db.KhachHangs.Remove(khachHang);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
