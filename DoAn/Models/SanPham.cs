namespace DoAn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            GioHangs = new HashSet<GioHang>();
            DonHangChiTiets = new HashSet<DonHangChiTiet>();
        }

        [Key]
        public int masp { get; set; }

        public int mahang { get; set; }

        [Required]
        [StringLength(500)]
        public string tensp { get; set; }

        [StringLength(500)]
        public string hinhanh { get; set; }

        public int? soluong { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0}")]
        [DataType(DataType.Currency)]
        public decimal? giaban { get; set; }

        [StringLength(4000)]
        public string mota { get; set; }

        [StringLength(500)]
        public string CPU { get; set; }

        [StringLength(500)]
        public string RAM { get; set; }

        [StringLength(500)]
        public string OS { get; set; }

        [StringLength(500)]
        public string manhinh { get; set; }

        [StringLength(200)]
        public string carddohoa { get; set; }

        [StringLength(200)]
        public string model {  get; set; }

        [StringLength(500)]
        public string SSD { get; set; }

        //public bool? sale {  get; set; }

        public bool? trangthai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioHang> GioHangs { get; set; }

        public virtual HangSP HangSP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHangChiTiet> DonHangChiTiets { get; set; }
    }
}
