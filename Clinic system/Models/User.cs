using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_system.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        // User Full name
        [Required(ErrorMessage ="* Required")]
        [StringLength(40,MinimumLength =6,ErrorMessage ="Name Must be between 6 and 40 Characters.")]
        public string FullName { get; set; }

        // User Email
        [Required(ErrorMessage = "* Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        // User Password
        [Required(ErrorMessage ="* Required")]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Password Must be between 8 and 40 Characters.")]
        public string Password { get; set; }

        // User Phone number
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        // User Gender
        public string? Gender { get; set; }

        // User Birth Date
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        // User Role
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }
    }
}
