using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class DonHangsController : Controller
    {
        private Model1 db = new Model1();

        // GET: DonHangs
        public ActionResult Index()
        {
            List<DonHang> donHangs = db.DonHangs.Include(d => d.KhachHang).ToList();
            donHangs = donHangs.OrderByDescending(d => d.madonhang).ToList();
            return View(donHangs);
        }

        // GET: DonHangs/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DonHang donHang = db.DonHangs.Find(id);
        //    if (donHang == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(donHang);
        //}

        //// GET: DonHangs/Create
        //public ActionResult Create()
        //{
        //    ViewBag.makhachhang = new SelectList(db.KhachHangs, "makhachhang", "hoten");
        //    return View();
        //}

        //// POST: DonHangs/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "madonhang,makhachhang,dienthoai,diachi,ngaydat,ngaynhan,thanhtoan,soluongmua,tongtien,trangthai")] DonHang donHang)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.DonHangs.Add(donHang);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.makhachhang = new SelectList(db.KhachHangs, "makhachhang", "hoten", donHang.makhachhang);
        //    return View(donHang);
        //}

        //// GET: DonHangs/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DonHang donHang = db.DonHangs.Find(id);
        //    if (donHang == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.makhachhang = new SelectList(db.KhachHangs, "makhachhang", "hoten", donHang.makhachhang);
        //    return View(donHang);
        //}

        //// POST: DonHangs/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "madonhang,makhachhang,dienthoai,diachi,ngaydat,ngaynhan,thanhtoan,soluongmua,tongtien,trangthai")] DonHang donHang)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(donHang).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.makhachhang = new SelectList(db.KhachHangs, "makhachhang", "hoten", donHang.makhachhang);
        //    return View(donHang);
        //}

        //// GET: DonHangs/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DonHang donHang = db.DonHangs.Find(id);
        //    if (donHang == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(donHang);
        //}

        //// POST: DonHangs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    DonHang donHang = db.DonHangs.Find(id);
        //    db.DonHangs.Remove(donHang);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        public ActionResult ChiTietDonHang(int? id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            List<DonHangChiTiet> listDHCT = new List<DonHangChiTiet>();
            listDHCT = db.DonHangChiTiets.Where(s => s.madonhang == id).ToList();
            ViewBag.DHCT = listDHCT;
            return View(donHang);
        }

        public ActionResult Huy(int id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            //List<DonHangChiTiet> listDHCT = new List<DonHangChiTiet>();
            //listDHCT = db.DonHangChiTiets.Where(s => s.madonhang == id).ToList();
            //listDHCT.Clear();
            //db.DonHangs.Remove(donHang);
            donHang.trangthai = null;
            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}
