using System.ComponentModel.DataAnnotations;

namespace Clinic_system.ViewModels
{
    public class TrackViewModel
    {
        [Required(ErrorMessage ="*مطلوب")]
        [DataType(DataType.PhoneNumber, ErrorMessage ="الهاتف غير صحيح")]
        [StringLength(16,MinimumLength =6,ErrorMessage = "الهاتف غير صحيح")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "*مطلوب")]
        [DataType(DataType.EmailAddress, ErrorMessage = "الإيميل غير صحيح")]
        public string EmailAddress { get; set; }

        [StringLength(5,MinimumLength = 5,ErrorMessage ="الرقم غير صحيح")]
        public string? Otp {  get; set; } 
    }
}
