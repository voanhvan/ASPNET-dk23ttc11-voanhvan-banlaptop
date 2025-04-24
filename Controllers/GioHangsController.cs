using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;
using DoAn.PayMethod;

namespace DoAn.Controllers
{
    public class GioHangsController : Controller
    {
        private Model1 db = new Model1();

        // GET: GioHangs
        public ActionResult Index()
        {
            var gioHangs = db.GioHangs.Include(g => g.KhachHang).Include(g => g.SanPham);
            return View(gioHangs.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult RemoveCart(int magiohang)
        {
            GioHang gh = db.GioHangs.Where(s=>s.magiohang == magiohang).FirstOrDefault();
            db.GioHangs.Remove(gh);
            db.SaveChanges();
            Session["giohang"] = (int)Session["giohang"] - 1;
            return Json(new { success = true });
        }

        public ActionResult UpdateCart([Bind(Include = "magiohang,masp,makhachhang,soluong")] GioHang gioHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gioHang).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
                
            }
            return RedirectToAction("Index");
        }

        public ActionResult ThanhToan()
        {
            var gioHangs = db.GioHangs.Include(g => g.KhachHang).Include(g => g.SanPham);
            KhachHang taikhoan = db.KhachHangs.Find((int)Session["ma"]);
            ViewBag.taikhoan = taikhoan;
            return View(gioHangs.ToList());
        }

        public ActionResult DatHang()
        {
            
            KhachHang taikhoan = db.KhachHangs.Find((int)Session["ma"]);
            List<GioHang> dssp = db.GioHangs.Where(s => s.makhachhang == taikhoan.makhachhang).ToList();
            decimal tongtien = 0;
            foreach (var item in dssp)
            {
                tongtien = tongtien + (decimal)(item.SanPham.giaban * item.soluong);
            }
            DonHang donHang = new DonHang();
            //string thanhtoan = Request.Form["thanhtoan"];
            //if (thanhtoan == "cod")
            //{
            //    donHang.thanhtoan = "Thanh toán khi nhận hàng";
            //}
            //else if(thanhtoan == "banking")
            //{
            //    donHang.thanhtoan = "Chuyển Khoản";
            //}
            donHang.makhachhang = taikhoan.makhachhang;
            
            donHang.diachi = Request.Form["diachi"] +", " + Request.Form["phuongxa"] + ", " + Request.Form["huyen"] + "," + Request.Form["thanhpho"];
            donHang.tongtien = tongtien;
            donHang.trangthai = false;
            donHang.thanhtoan = "Thanh toán khi nhận hàng";
            donHang.ngaydat = DateTime.Now;
            donHang.ngaynhan = donHang.ngaydat.AddDays(3);     
            taikhoan.diachi = Request.Form["dienthoai"];
            taikhoan.tinh = Request.Form["thanhpho"];
            taikhoan.huyen = Request.Form["huyen"];
            taikhoan.xa = Request.Form["phuongxa"];
            taikhoan.thon = Request.Form["diachi"];
            donHang.dienthoai = Request.Form["dienthoai"];
            taikhoan.dienthoai = donHang.dienthoai;
            db.DonHangs.Add(donHang);
            db.SaveChanges();
            var strSanPham = "";
            foreach (var item in dssp)
            {
                DonHangChiTiet dhct = new DonHangChiTiet();
                dhct.madonhang = donHang.madonhang;
                dhct.masp = item.masp;
                dhct.soluong = item.soluong;
                dhct.gia = item.SanPham.giaban;
                var sanpham = db.SanPhams.Find(item.masp);
                sanpham.soluong = sanpham.soluong - item.soluong;

                //data to send mail
                strSanPham += "<tr>";
                strSanPham += "<td>" + item.SanPham.tensp + "</td>";
                strSanPham += "<td>" + item.soluong + "</td>";
                strSanPham += "<td>" + item.SanPham.giaban + "</td>";
                strSanPham += "</tr>";

                db.GioHangs.Remove(item);
                db.DonHangChiTiets.Add(dhct);
                db.SaveChanges();
            }
            Session["giohang"] = 0;

            //send mail
            string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/Template/send2.html"));
            contentCustomer = contentCustomer.Replace("{{madon}}", donHang.madonhang.ToString());
            contentCustomer = contentCustomer.Replace("{{sanpham}}", strSanPham);
            contentCustomer = contentCustomer.Replace("{{hoten}}", taikhoan.hoten);
            contentCustomer = contentCustomer.Replace("{{ngaydat}}", donHang.ngaydat.ToString());
            contentCustomer = contentCustomer.Replace("{{tongtien}}", tongtien.ToString("#,##0"));
            contentCustomer = contentCustomer.Replace("{{diachi}}", donHang.diachi);
            contentCustomer = contentCustomer.Replace("{{dienthoai}}", taikhoan.dienthoai);
            contentCustomer = contentCustomer.Replace("{{email}}", taikhoan.email);
            DoAn.Common.MailHeper.sendEmail("An Phát Computer", "Đơn Hàng #" + donHang.madonhang, contentCustomer.ToString(), taikhoan.email);
            //string url = UrlPayment(1, donHang.madonhang);

            return RedirectToAction("Index", "DonHangs");
            //return Json(new { success = true });
        }

        public ActionResult DatHang1()
        {

            List<DonHangChiTiet> dhct = db.DonHangChiTiets.ToList();
            foreach (var item in dhct)
            {
                db.DonHangChiTiets.Remove(item);
            }
            List<DonHang> dh = db.DonHangs.ToList();
            foreach (var item in dh)
            {
                db.DonHangs.Remove(item);
            }
            db.SaveChanges();

            return RedirectToAction("Index", "DonHangs");
        }

        public string UrlPayment(int typePayment, int orderID)
        {
            var urlPayment = "";
            DonHang donHang = db.DonHangs.FirstOrDefault(d => d.madonhang == orderID);

            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (donHang.tongtien * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            
            if (typePayment == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (typePayment == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (typePayment == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", donHang.ngaydat.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang: #DH" + donHang.madonhang);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", donHang.madonhang.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return urlPayment;
        }

        public ActionResult VnpayReturn()
        {
            return View();
        }

    }
}
