using System.ComponentModel.DataAnnotations;

namespace AFRICOMDMSWEB.Models.ViewModel
{
    public class LoginVm
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }

    }
}
