using System.ComponentModel.DataAnnotations;
using System;

namespace TSJ_CRI.Model
{
    public class UserFormHR
    {
        public int No_id { get; set; }

        [Required(ErrorMessage = "Kode tidak dapat kosong")]
        public string Kode { get; set; }
        public string Namaproduk { get; set; }

        public string Batch { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd/MM/yyyy")]
        public DateTime? Expiredate { get; set; }= DateTime.Now;

        public int Jumlah { get; set; }
        public string Ket { get; set; }

        public int User_id { get; set; }

        public string id_temp { get; set; }

        public int HNA { get; set; }
        
    }
    public class UserFormHRbaru
    {
        public int No_id { get; set; }

        [Required(ErrorMessage = "Kode tidak dapat kosong")]
        public string Kode { get; set; }
        [Required(ErrorMessage = "Nama Produk tidak dapat kosong")]
        public string Namaproduk { get; set; }
        [Required(ErrorMessage = "Batch tidak dapat kosong")]
        public string Batch { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd/MM/yyyy")]
        public DateTime? Expiredate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Jumlah Barang tidak dapat kosong")]
        public int Jumlah { get; set; }
        public string Ket { get; set; }
        public int User_id { get; set; }

        public string id_temp { get; set; }
        
    }
}
