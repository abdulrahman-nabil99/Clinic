using System.ComponentModel.DataAnnotations;

namespace Clinic_system.ViewModels
{
    public class BookViewModel
    {
        [Required(ErrorMessage = " * مطلوب")]
        [StringLength(50,MinimumLength =3, ErrorMessage ="الإسم يجب أن يكون أكثر من 3 حروف ولا يتجاوز 50 حرف")]
        public string Name { get; set; }

        [Required(ErrorMessage = " * مطلوب")]
        [DataType(DataType.PhoneNumber,ErrorMessage ="رقم الهاتف غير صالح")]
        public string Phone {  get; set; }

        [DataType(DataType.EmailAddress,ErrorMessage ="الإيميل غير صالح")]
        [Required(ErrorMessage = " * مطلوب")]
        public string Email {  get; set; }

        [Required(ErrorMessage = " * مطلوب")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = " * مطلوب")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
