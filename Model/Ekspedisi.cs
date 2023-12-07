
using System.ComponentModel.DataAnnotations;

namespace TSJ_CRI.Model
{
    public class Ekspedisi
    {
        [Required(ErrorMessage = "tidak dapat kosong")]
        public string NAMA_EKSPEDISI { get; set; }
        public string STATUS { get; set; }

        public string LAST_UPDATE_DATE { get; set; }

        public string email { get; set; }

        public string email_2 { get; set; }

        public string email_3 { get; set; }
    }
}
