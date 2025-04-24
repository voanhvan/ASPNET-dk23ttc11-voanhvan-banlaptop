namespace DoAn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatemodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DonHangChiTiet",
                c => new
                    {
                        machitietdonhang = c.Int(nullable: false),
                        madonhang = c.Int(nullable: false),
                        masp = c.Int(nullable: false),
                        soluong = c.Int(),
                        gia = c.Decimal(precision: 18, scale: 0),
                        tongtien = c.Decimal(precision: 18, scale: 0),
                    })
                .PrimaryKey(t => new { t.machitietdonhang, t.madonhang, t.masp })
                .ForeignKey("dbo.DonHang", t => t.madonhang)
                .ForeignKey("dbo.SanPham", t => t.masp)
                .Index(t => t.madonhang)
                .Index(t => t.masp);
            
            CreateTable(
                "dbo.DonHang",
                c => new
                    {
                        madonhang = c.Int(nullable: false, identity: true),
                        makhachhang = c.Int(nullable: false),
                        dienthoai = c.String(maxLength: 15, fixedLength: true),
                        diachi = c.String(maxLength: 500),
                        ngaydat = c.DateTime(nullable: false, storeType: "date"),
                        ngaynhan = c.DateTime(nullable: false, storeType: "date"),
                        thanhtoan = c.String(nullable: false, maxLength: 150),
                        soluongmua = c.Int(),
                        tongtien = c.Decimal(precision: 18, scale: 0),
                        trangthai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.madonhang)
                .ForeignKey("dbo.KhachHang", t => t.makhachhang)
                .Index(t => t.makhachhang);
            
            CreateTable(
                "dbo.KhachHang",
                c => new
                    {
                        makhachhang = c.Int(nullable: false, identity: true),
                        hoten = c.String(nullable: false, maxLength: 50),
                        diachi = c.String(maxLength: 500),
                        dienthoai = c.String(maxLength: 15, fixedLength: true),
                        email = c.String(maxLength: 100, fixedLength: true),
                        matkhau = c.String(maxLength: 10, fixedLength: true),
                        trangthai = c.Boolean(),
                        chucvu = c.Boolean(),
                    })
                .PrimaryKey(t => t.makhachhang);
            
            CreateTable(
                "dbo.GioHang",
                c => new
                    {
                        magiohang = c.Int(nullable: false, identity: true),
                        masp = c.Int(nullable: false),
                        makhachhang = c.Int(nullable: false),
                        soluong = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.magiohang)
                .ForeignKey("dbo.SanPham", t => t.masp)
                .ForeignKey("dbo.KhachHang", t => t.makhachhang)
                .Index(t => t.masp)
                .Index(t => t.makhachhang);
            
            CreateTable(
                "dbo.SanPham",
                c => new
                    {
                        masp = c.Int(nullable: false, identity: true),
                        mahang = c.Int(nullable: false),
                        tensp = c.String(nullable: false, maxLength: 500),
                        hinhanh = c.String(maxLength: 500),
                        soluong = c.Int(),
                        giaban = c.Decimal(precision: 18, scale: 0),
                        mota = c.String(maxLength: 4000),
                        CPU = c.String(maxLength: 500),
                        RAM = c.String(maxLength: 500),
                        OS = c.String(maxLength: 50),
                        manhinh = c.String(maxLength: 500),
                        carddohoa = c.String(maxLength: 200),
                        SSD = c.String(maxLength: 50, fixedLength: true),
                        trangthai = c.Boolean(),
                    })
                .PrimaryKey(t => t.masp)
                .ForeignKey("dbo.HangSP", t => t.mahang)
                .Index(t => t.mahang);
            
            CreateTable(
                "dbo.HangSP",
                c => new
                    {
                        mahang = c.Int(nullable: false, identity: true),
                        tenhang = c.String(nullable: false, maxLength: 50),
                        trangthai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.mahang);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GioHang", "makhachhang", "dbo.KhachHang");
            DropForeignKey("dbo.SanPham", "mahang", "dbo.HangSP");
            DropForeignKey("dbo.GioHang", "masp", "dbo.SanPham");
            DropForeignKey("dbo.DonHangChiTiet", "masp", "dbo.SanPham");
            DropForeignKey("dbo.DonHang", "makhachhang", "dbo.KhachHang");
            DropForeignKey("dbo.DonHangChiTiet", "madonhang", "dbo.DonHang");
            DropIndex("dbo.SanPham", new[] { "mahang" });
            DropIndex("dbo.GioHang", new[] { "makhachhang" });
            DropIndex("dbo.GioHang", new[] { "masp" });
            DropIndex("dbo.DonHang", new[] { "makhachhang" });
            DropIndex("dbo.DonHangChiTiet", new[] { "masp" });
            DropIndex("dbo.DonHangChiTiet", new[] { "madonhang" });
            DropTable("dbo.HangSP");
            DropTable("dbo.SanPham");
            DropTable("dbo.GioHang");
            DropTable("dbo.KhachHang");
            DropTable("dbo.DonHang");
            DropTable("dbo.DonHangChiTiet");
        }
    }
}
