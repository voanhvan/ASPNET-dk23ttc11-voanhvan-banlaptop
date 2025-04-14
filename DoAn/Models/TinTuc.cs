using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAn.Models
{
    [Table("TinTuc")]
    public partial class TinTuc
    {
        [Key]
        public int matintuc {  get; set; }

        [StringLength(500)]
        public string tieude { get; set; }

        [StringLength(50)]
        public string hinhanh { get; set; }

        public DateTime ngaytao { get; set; }
        public string gioithieu {  get; set; }

        public bool? trangthai { get; set; }
        public int makhachhang { get; set; }
        public string noidung {  get; set; }

        public virtual KhachHang KhachHang { get; set; }
    }
}