namespace DoAn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhachHang")]
    public partial class KhachHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhachHang()
        {
            DonHangs = new HashSet<DonHang>();
            GioHangs = new HashSet<GioHang>();
        }

        [Key]
        public int makhachhang { get; set; }

        [Required]
        [StringLength(50)]
        public string hoten { get; set; }

        [StringLength(500)]
        public string diachi { get; set; }

        [StringLength(15)]
        public string dienthoai { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [Column(TypeName = "date")]
        public DateTime ngaydangky { get; set; }

        [StringLength(500)]
        public string tinh { get; set; }

        [StringLength(500)]
        public string huyen { get; set; }

        [StringLength(500)]
        public string xa { get; set; }

        [StringLength(500)]
        public string thon { get; set; }

        [StringLength(10)]
        public string matkhau { get; set; }

        public bool? trangthai { get; set; }

        public bool? chucvu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioHang> GioHangs { get; set; }
        public virtual ICollection<TinTuc> TinTucs { get; set; }
    }
}
