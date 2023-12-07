using System.ComponentModel.DataAnnotations;

namespace TSJ_CRI.Model
{
    
    public class hilangrusak
    {
        [Required(ErrorMessage = "Pilih Salah satu")]
        public string listhilangrusak { get; set; }
        [Required(ErrorMessage = "Pilih Salah satu")]
        public string Description { get; set; }
    }
}
