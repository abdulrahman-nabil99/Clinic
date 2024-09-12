using System.ComponentModel.DataAnnotations;

namespace Clinic_system.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="* مطلوب")]
        [StringLength(90,MinimumLength =5,ErrorMessage = "العنوان غير مناسب")]
        public string Title { get; set; }

        [Required(ErrorMessage = "* مطلوب")]
        public string Content { get; set; }

        [Required(ErrorMessage = "* مطلوب")]
        [DataType(DataType.ImageUrl)]
        public string Thumbnail { get; set; } = "default.png";
    }
}
