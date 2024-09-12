using System.ComponentModel.DataAnnotations;

namespace Clinic_system.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "* Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
