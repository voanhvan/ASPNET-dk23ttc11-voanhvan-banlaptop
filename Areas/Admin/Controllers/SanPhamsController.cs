using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.Services.Description;
using DoAn.Models;
using PagedList;

namespace DoAn.Areas.Admin.Controllers
{
    public class SanPhamsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Admin/SanPhams
        public ActionResult Index(int ? page)
        {
            var searchString = Request.Form["search"];
            ViewBag.CurrentFilter = searchString;

            var sanPhams = db.SanPhams.Include(s => s.HangSP).ToList();
            sanPhams = sanPhams.OrderByDescending(s => s.masp).ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                sanPhams = db.SanPhams.Where(sp => sp.tensp.Contains(searchString)).ToList();
            }
            
            int pageSize = 10; // Số lượng dữ liệu trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, nếu không được cung cấp thì mặc định là trang 1

            // Truy vấn dữ liệu từ nguồn dữ liệu và tạo đối tượng PagedList
            return View(sanPhams.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/SanPhams/Details/5
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
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.mahang = new SelectList(db.HangSPs, "mahang", "tenhang");
            List<SelectListItem> trangThaiList = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Hiển thị" },
                new SelectListItem { Value = "false", Text = "Ẩn" }
            };
            ViewBag.TrangThaiList = trangThaiList;
            return View();
        }

        // POST: Admin/SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "masp,mahang,tensp,hinhanh,soluong,giaban,mota,CPU,RAM,OS,manhinh,carddohoa,SSD,trangthai,model")] SanPham sanPham)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sanPham.hinhanh = "";
                    var file = Request.Files["ImageUpload"];
                    if (file != null && file.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(file.FileName);
                        string filepath = Server.MapPath("~/wwwroot/images/" + filename);
                        file.SaveAs(filepath);
                        sanPham.hinhanh = filename;
                    }

                    db.SanPhams.Add(sanPham);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi nhập dữ liệu: " + ex.Message;
                ViewBag.mahang = new SelectList(db.HangSPs, "mahang", "tenhang");
                return View(sanPham);
            }
        }

        // GET: Admin/SanPhams/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.mahang = new SelectList(db.HangSPs, "mahang", "tenhang", sanPham.mahang);
            List<SelectListItem> trangThaiList = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Hiển thị" },
                new SelectListItem { Value = "false", Text = "Ẩn" }
            };
            ViewBag.TrangThaiList = trangThaiList;
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "masp,mahang,tensp,hinhanh,soluong,giaban,mota,CPU,RAM,OS,manhinh,carddohoa,SSD,trangthai,model")] SanPham sanPham)
        {
            try
            {
                
                if (ModelState.IsValid)
                {   
                    var file = Request.Files["ImageUpload"];
                    if (file != null && file.ContentLength > 0)
                    {
                        sanPham.hinhanh = "";
                        string filename = Path.GetFileName(file.FileName);
                        string filepath = Server.MapPath("~/wwwroot/images/" + filename);
                        file.SaveAs(filepath);
                        sanPham.hinhanh = filename;
                    }
                    
                    db.Entry(sanPham).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi nhập dữ liệu: " + ex.Message;
                ViewBag.mahang = new SelectList(db.HangSPs, "mahang", "tenhang", sanPham.mahang);
                return View(sanPham);
            }
        }

        // GET: Admin/SanPhams/Delete/5
        public ActionResult Delete(int? id)
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
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
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
