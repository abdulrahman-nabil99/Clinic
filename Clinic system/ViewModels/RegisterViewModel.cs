using System.ComponentModel.DataAnnotations;

namespace Clinic_system.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "* Required")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "Name Must be between 6 and 40 Characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password Must be between 8 and 40 Characters.")]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
    }
}
