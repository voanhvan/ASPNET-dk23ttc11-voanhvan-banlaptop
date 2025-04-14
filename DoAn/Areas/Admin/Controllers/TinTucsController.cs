using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;

namespace DoAn.Areas.Admin.Controllers
{
    public class TinTucsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Admin/TinTucs
        public ActionResult Index()
        {
            var tinTucs = db.TinTucs.ToList();
            tinTucs = tinTucs.OrderByDescending(n => n.matintuc).ToList();
            return View(tinTucs);
        }

        // GET: Admin/TinTucs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TinTuc tinTuc = db.TinTucs.Find(id);
            if (tinTuc == null)
            {
                return HttpNotFound();
            }
            return View(tinTuc);
        }

        // GET: Admin/TinTucs/Create
        public ActionResult Create()
        {
            ViewBag.makhachhang = new SelectList(db.KhachHangs.Where(s => s.chucvu == true), "makhachhang", "hoten");
            List<SelectListItem> trangThaiList = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Hiển thị" },
                new SelectListItem { Value = "false", Text = "Ẩn" }
            };
            ViewBag.TrangThaiList = trangThaiList;
            return View();

        }

        // POST: Admin/TinTucs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "matintuc,tieude,hinhanh,ngaytao,gioithieu,trangthai,makhachhang,noidung")] TinTuc tinTuc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tinTuc.hinhanh = "";
                    var file = Request.Files["ImageUpload"];
                    if (file != null && file.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(file.FileName);
                        string filepath = Server.MapPath("~/wwwroot/tintuc/" + filename);
                        file.SaveAs(filepath);
                        tinTuc.hinhanh = filename;
                    }
                    tinTuc.ngaytao = DateTime.Now;
                    db.TinTucs.Add(tinTuc);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.makhachhang = new SelectList(db.KhachHangs, "makhachhang", "hoten", tinTuc.makhachhang);
                return View(tinTuc);
            }
        }

        // GET: Admin/TinTucs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TinTuc tinTuc = db.TinTucs.Find(id);
            if (tinTuc == null)
            {
                return HttpNotFound();
            }
            ViewBag.makhachhang = new SelectList(db.KhachHangs.Where(s => s.chucvu == true), "makhachhang", "hoten", tinTuc.makhachhang);
            List<SelectListItem> trangThaiList = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Hiển thị" },
                new SelectListItem { Value = "false", Text = "Ẩn" }
            };
            ViewBag.TrangThaiList = trangThaiList;
            return View(tinTuc);
        }

        // POST: Admin/TinTucs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "matintuc,tieude,hinhanh,ngaytao,gioithieu,trangthai,makhachhang,noidung")] TinTuc tinTuc)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var file = Request.Files["ImageUpload"];
                    if (file != null && file.ContentLength > 0)
                    {
                        tinTuc.hinhanh = "";
                        string filename = Path.GetFileName(file.FileName);
                        string filepath = Server.MapPath("~/wwwroot/tintuc/" + filename);
                        file.SaveAs(filepath);
                        tinTuc.hinhanh = filename;
                    }
                    db.Entry(tinTuc).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.makhachhang = new SelectList(db.KhachHangs, "makhachhang", "hoten", tinTuc.makhachhang);
                return View(tinTuc);

            }
        }

        // GET: Admin/TinTucs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TinTuc tinTuc = db.TinTucs.Find(id);
            if (tinTuc == null)
            {
                return HttpNotFound();
            }
            return View(tinTuc);
        }

        // POST: Admin/TinTucs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TinTuc tinTuc = db.TinTucs.Find(id);
            db.TinTucs.Remove(tinTuc);
            db.SaveChanges();
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
