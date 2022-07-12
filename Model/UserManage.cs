using System.ComponentModel.DataAnnotations;

namespace TSJ_CRI.Model
{
    public class UserManage
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Username tidak dapat kosong")]
        [MaxLength(100, ErrorMessage = "Panjang maksimum tidak lebih dari 100 karakter")]
        [MinLength(5, ErrorMessage = "panjang maksimum minimal 6 karakter")]
        public string username { get; set; }

        [Required(ErrorMessage = "Email tidak dapat kosong")]
        public string email { get; set; }
        public string roles { get; set; }
        public string org_id { get; set; }
        public string status { get; set; }
        public string cabang { get; set; }
    }

    public class UserNew
    {
        [Required(ErrorMessage = "Wajib Pilih cabang")]
        public string org_id { get; set; }
        
        public string username { get; set; }
        [Required(ErrorMessage = "Wajib pilih roles")]
        public string roles { get; set; }
        public string email { get; set; }
    }

}
