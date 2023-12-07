using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
namespace TSJ_CRI.Model
{
    public class UserForm
    {


        public string no_header;
        public int no_id { get; set; }


        [Required(ErrorMessage = "Tipe tidak dapat kosong")]
        public string hilangrusak { get; set; }

        [Required(ErrorMessage = "Nama Ekspedisi tidak dapat kosong")]
        public string namaekspedisi { get; set; }

        
        [Required(ErrorMessage = "No Kendaraan tidak dapat kosong")]

        public string nokendaraan { get; set; }
        [Required(ErrorMessage = "Nama Supir tidak dapat kosong ")]
        public string namasupir { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "No Pengiriman harus berupa angka")]
        [Required(ErrorMessage = "No tidak dapat kosong")]
        public string nopengiriman { get; set; }
        
        public string pengirimandari { get; set; }
        
       
        public string pengirimanke { get; set; }
        
        public string nodokument { get; set; }
        public string koli { get; set; }
       
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd/MM/yyyy")]

        public DateTime? tglkirim { get; set; } = DateTime.Today.Date;

        public string id_temp { get; set; }

        public string UpdateHR { get; set; }

        public string FILENAME { get; set; }
        public string no_pengajuan { get; set; }

        public DateTime create_date { get; set; } = DateAndTime.Now;

        public string download { get; set; }
        public int DHR { get; set; }
        public string branch_name { get; set; }

        public int HNA { get; set; }

        public string Approve { get; set; }

        public string FA { get; set; }
    }
    public class UserFormbaru
    {
        public int no_id { get; set; }
        
       
        public string hilangrusak { get; set; }
        
        public string namaekspedisi { get; set; }


        
        [Required(ErrorMessage = "No kendaraan tidak dapat kosong")]
        public string nokendaraan { get; set; }
        [Required(ErrorMessage = "Nama tidak dapat kosong ")]
        public string namasupir { get; set; }
        
        public string nopengiriman { get; set; }
        
        public string pengirimandari { get; set; }
        
        public string pengirimanke { get; set; }
        
        public string nodokument { get; set; }
        public string koli { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd/MM/yyyy")]
        public DateTime? tglkirim { get; set; } = DateTime.Today.Date;

        public string id_temp { get; set; }
        public string UpdateHR { get; set; }

        public string FILENAME { get; set; }
        public string no_pengajuan { get; set; }

        public DateTime create_date { get; set; } = DateAndTime.Now;
        public string download { get; set; }

        public int DHR { get; set; }
        public string branch_name { get; set; }
        public int HNA { get; set; }
        public string Approve { get; set; }

        public string FA { get; set; }
    }
}
