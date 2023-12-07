using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.VisualBasic;

namespace TSJ_CRI.Model
{
    public class UserForm2
    {
        public int No_id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd/MM/yyyy")]
        public DateTime Tanggal { get; set; } = DateTime.Now;
        public string No { get; set; }
        
        [Required(ErrorMessage = "No tidak dapat kosong")]
        public string PT { get; set; }
        [Required(ErrorMessage = "Nama PT tidak dapat kosong")]
        public string Ketidaksesuaian { get; set; }
        [Required(ErrorMessage = "Ketidaksesuaian tidak dapat kosong")]
        public string Notes { get; set; }

        public string UpdateTS { get; set; }
        
        public string FILENAME { get; set; }

        public string id_temp { get; set; }

        public string no_pengajuan { get; set; }

        public DateTime create_date { get; set; } = DateAndTime.Now;
        public string download { get; set; }
        public string branch_name { get; set; }
        public int HNA { get; set; }

        public string Approve { get; set; }

        public string FA { get; set; }

    }
    public class UserFormbaru2
    {
        public int No_id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd/MM/yyyy")]
        public DateTime Tanggal { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "No tidak dapat kosong")]
        public string PT { get; set; }
        [Required(ErrorMessage = "PT tidak dapat kosong")]

        public string Ketidaksesuaian { get; set; }
        [Required(ErrorMessage = "Notes tidak dapat kosong")]
        public string Notes { get; set; }

        public string UpdateTS { get; set; }

        public string FILENAME { get; set; }

        public string id_temp { get; set; }
        public string no_pengajuan { get; set; }

        public DateTime create_date { get; set; } = DateAndTime.Now;

        public string download { get; set; }

        public string branch_name { get; set; }
        public int HNA { get; set; }

        public string Approve { get; set; }
        public string FA { get; set; }
    }
}
