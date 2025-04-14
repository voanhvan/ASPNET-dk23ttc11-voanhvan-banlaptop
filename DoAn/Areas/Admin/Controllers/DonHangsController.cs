using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DoAn.Models;
using PagedList;

namespace DoAn.Areas.Admin.Controllers
{
    public class DonHangsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Admin/DonHangs
        public ActionResult Index(int? page)
        {
            //var donHangs = db.DonHangs.Include(d => d.KhachHang);
            var searchString = Request.Form["search"];
            ViewBag.CurrentFilter = searchString;

            var donhangs = db.DonHangs.Include(s => s.KhachHang).ToList();
            donhangs = donhangs.OrderByDescending(s =>s.madonhang).ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                donhangs = db.DonHangs.Where(sp => sp.dienthoai.Equals(searchString)).ToList();
            }

            int pageSize = 15; // Số lượng dữ liệu trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, nếu không được cung cấp thì mặc định là trang 1

            // Truy vấn dữ liệu từ nguồn dữ liệu và tạo đối tượng PagedList
            return View(donhangs.ToPagedList(pageNumber, pageSize));
            //return View(donHangs.ToList());
        }


        // GET: Admin/DonHangs/Details/5
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            List<DonHangChiTiet> listDHCT = new List<DonHangChiTiet>();
            listDHCT = db.DonHangChiTiets.Where(s => s.madonhang == id).ToList();
            ViewBag.DHCT = listDHCT;
            return View(donHang);
        }

        // GET: Admin/DonHangs/Create
        public ActionResult Create()
        {
            ViewBag.makhachhang = new SelectList(db.KhachHangs, "makhachhang", "hoten");
            return View();
        }

        // POST: Admin/DonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "madonhang,makhachhang,dienthoai,diachi,ngaydat,ngaynhan,thanhtoan,soluongmua,tongtien,trangthai")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.DonHangs.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.makhachhang = new SelectList(db.KhachHangs, "makhachhang", "hoten", donHang.makhachhang);
            return View(donHang);
        }

        // GET: Admin/DonHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.makhachhang = new SelectList(db.KhachHangs, "makhachhang", "hoten", donHang.makhachhang);
            return View(donHang);
        }

        // POST: Admin/DonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "madonhang,makhachhang,dienthoai,diachi,ngaydat,ngaynhan,thanhtoan,soluongmua,tongtien,trangthai")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.makhachhang = new SelectList(db.KhachHangs, "makhachhang", "hoten", donHang.makhachhang);
            return View(donHang);
        }

        // GET: Admin/DonHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: Admin/DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            db.DonHangs.Remove(donHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult UpdateOrder(int id)
        {
            var donHang = db.DonHangs.Find(id);
            donHang.trangthai = true;
            db.SaveChanges();
            return Json(new {success = true});
        }
    }
}
