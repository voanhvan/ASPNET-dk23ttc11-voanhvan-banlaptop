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
    public class HangSPsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Admin/HangSPs
        public ActionResult Index(int ? page)
        {
            var hangSanPhams = db.HangSPs.ToList();
            int pageSize = 10; // Số lượng dữ liệu trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, nếu không được cung cấp thì mặc định là trang 1

            // Truy vấn dữ liệu từ nguồn dữ liệu và tạo đối tượng PagedList
            return View(hangSanPhams.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/HangSPs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/HangSPs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mahang,tenhang,trangthai")] HangSP hangSP)
        {
            if (ModelState.IsValid)
            {
                hangSP.trangthai = true;
                db.HangSPs.Add(hangSP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hangSP);
        }

        // GET: Admin/HangSPs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HangSP hangSP = db.HangSPs.Find(id);
            if (hangSP == null)
            {
                return HttpNotFound();
            }
            return View(hangSP);
        }

        // POST: Admin/HangSPs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mahang,tenhang,trangthai")] HangSP hangSP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hangSP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hangSP);
        }

        // GET: Admin/HangSPs/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    HangSP hangSP = db.HangSPs.Find(id);
        //    if (hangSP == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(hangSP);
        //}

        //// POST: Admin/HangSPs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        { 
            List<SanPham> phamList = db.SanPhams.Where(sp => sp.mahang == id).ToList();
            foreach (var pham in phamList)
            {
                db.SanPhams.Remove(pham);
            }
            HangSP hangSP = db.HangSPs.Find(id);
            db.HangSPs.Remove(hangSP);
            db.SaveChanges();
            return Json(new {success = true});
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
