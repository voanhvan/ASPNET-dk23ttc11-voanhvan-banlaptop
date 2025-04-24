namespace DoAn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonHang")]
    public partial class DonHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonHang()
        {
            DonHangChiTiets = new HashSet<DonHangChiTiet>();
        }

        [Key]
        public int madonhang { get; set; }

        public int makhachhang { get; set; }

        [StringLength(15)]
        public string dienthoai { get; set; }

        [StringLength(500)]
        public string diachi { get; set; }

        [Column(TypeName = "date")]
        public DateTime ngaydat { get; set; }

        [Column(TypeName = "date")]
        public DateTime ngaynhan { get; set; }

        [Required]
        [StringLength(150)]
        public string thanhtoan { get; set; }

        public int? soluongmua { get; set; }

        public decimal? tongtien { get; set; }

        public bool? trangthai { get; set; }

        public virtual KhachHang KhachHang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHangChiTiet> DonHangChiTiets { get; set; }
    }
}
