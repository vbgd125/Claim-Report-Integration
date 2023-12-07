
using System.ComponentModel.DataAnnotations;

namespace TSJ_CRI.Model
{
    public class UserKetidaksesuaian
    {
        [Required(ErrorMessage = "No tidak dapat kosong")]
        public int No_id { get; set; }
        [Required(ErrorMessage = "Dokumen tidak dapat kosong")]
        public string Dokumen { get; set; }

        [Required(ErrorMessage = "No PO tidak dapat kosong")]
        public string PO { get; set; }
        [Required(ErrorMessage = "Nama tidak dapat kosong")]

        public string kode { get; set; }
        public string Nama { get; set; }
        [Required(ErrorMessage = "TSdokumen tidak dapat kosong")]
        public int TSdokumen { get; set; }

        [Required(ErrorMessage = "TSproduk tidak dapat kosong")]
        public int TSproduk { get; set; }

        public int User_id { get; set; }

        public string id_temp { get; set; }

        public string UpdateTS { get; set; }

        public string FILENAME { get; set; }

        public int HNA { get; set; }


    }
    public class UserBaru
    {
        [Required(ErrorMessage = "No tidak dapat kosong")]
        public int No_id { get; set; }
        [Required(ErrorMessage = "Dokumen tidak dapat kosong")]
        public string Dokumen { get; set; }
        [Required(ErrorMessage = "No PO tidak dapat kosong")]
        public string PO { get; set; }
        public string kode { get; set; }
        [Required(ErrorMessage = "Nama tidak dapat kosong")]
        public string Nama { get; set; }
        [Required(ErrorMessage = "TSdokumen tidak dapat kosong")]
        public int TSdokumen { get; set; }
        [Required(ErrorMessage = "TSproduk tidak dapat kosong")]
        public int TSproduk { get; set; }
        public int User_id { get; set; }
        public string id_temp { get; set; }
        public string UpdateTS { get; set; }

        public string FILENAME { get; set; }

    }
}

