using System.ComponentModel.DataAnnotations;

namespace Clinic_system.Models
{
    public class Inquiry
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "* مطلوب")]
        [StringLength(30,MinimumLength =3, ErrorMessage ="الإسم غير صالح")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* مطلوب")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "الإيميل غير صالح")]
        [DataType(DataType.EmailAddress,ErrorMessage ="الإيميل غير صالح")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "* مطلوب")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "الرجاء توضيح الإستفسار بشكل أفضل")]
        public string Question { get; set; }

        public bool IsAnswered { get; set; } = false;

        [StringLength(500, MinimumLength = 1, ErrorMessage = "الإجابة غير صالحة")]
        public string? Answer { get; set; }

    }
}
